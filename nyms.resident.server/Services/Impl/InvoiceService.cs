﻿using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace nyms.resident.server.Services.Impl
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceDataProvider _invoiceDataProvider;
        private readonly IResidentDataProvider _residentDataProvider;
        private readonly IFeeCalculatorService _feeCalculatorService;
        private readonly IUserService _userService;

        public InvoiceService(IInvoiceDataProvider invoiceDataProvider, IResidentDataProvider residentDataProvider, IFeeCalculatorService feeCalculatorService, IUserService userService)
        {
            _invoiceDataProvider = invoiceDataProvider ?? throw new ArgumentException(nameof(invoiceDataProvider));
            _residentDataProvider = residentDataProvider ?? throw new ArgumentException(nameof(residentDataProvider));
            _feeCalculatorService = feeCalculatorService ?? throw new ArgumentNullException(nameof(feeCalculatorService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        // Data by Date Range
        public InvoiceData GetInvoiceData(DateTime startDate, DateTime endDate)
        {
            if (startDate == null) throw new ArgumentNullException(nameof(startDate));
            if (endDate == null) throw new ArgumentNullException(nameof(endDate));

            var invoiceResidents = this.GetInvoiceResidentData(startDate, endDate);
            var numOfDays = invoiceResidents.Select(d => d.SchedulePayments.Select(sp => sp.NumberOfDays)).FirstOrDefault().FirstOrDefault();

            return new InvoiceData()
            {
                BillingCycleId = 0,
                BeginDate = startDate,
                EndDate = endDate,
                BillingDate = DateTime.Now, // TODO
                NumberOfDays = numOfDays,
                InvoiceResidents = invoiceResidents
            };
        }

        // Data by billing cycle id
        public InvoiceData GetInvoiceData(int localAuthorityId, int billingCycleId)
        {
            var billingCycles = this.GetBillingCycles().Result;
            var billingCycle = billingCycles.Where(bc => bc.Id == billingCycleId).FirstOrDefault();
            if (billingCycle == null) throw new ArgumentNullException(nameof(billingCycle));

            var invoiceResidents = this.GetInvoiceResidentData(billingCycle.PeriodStart, billingCycle.PeriodEnd);
            var invoiceDataByLa = invoiceResidents.Where(d => d.LocalAuthorityId == localAuthorityId);
            var numOfDays = invoiceResidents.Select(d => d.SchedulePayments.Select(sp => sp.NumberOfDays)).FirstOrDefault().FirstOrDefault();

            // assemble validated date with invoice data
            var validatedInvoiceData = _invoiceDataProvider.GetValidatedInvoices((int)localAuthorityId, billingCycleId).Result;
            // get usernames for mapping
            var users = _userService.GetUsers();
            // get comments
            var comments = _invoiceDataProvider.GetInvoiceComments((int)localAuthorityId, billingCycleId).Result;

            invoiceDataByLa.ForEach(d =>
            {
                d.SchedulePayments.ForEach(sp =>
                {
                    var invoiceValidatedEntity = validatedInvoiceData.Where(ed => ed.LocalAuthorityId == sp.LocalAuthorityId && ed.PaymentTypeId == sp.PaymentTypeId && ed.ResidentId == sp.ResidentId).FirstOrDefault();
                    sp.InvoiceValidatedModel = new InvoiceValidatedModel();
                    if (invoiceValidatedEntity != null)
                    {
                        sp.InvoiceValidatedModel = new InvoiceValidatedModel()
                        {
                            Id = invoiceValidatedEntity.Id,
                            BillingCycleId = invoiceValidatedEntity.BillingCycleId,
                            PaymentTypeId = invoiceValidatedEntity.PaymentTypeId,
                            AmountDue = invoiceValidatedEntity.AmountDue,
                            Validated = invoiceValidatedEntity.Validated,
                            ValidatedAmount = invoiceValidatedEntity.ValidatedAmount,
                            UpdatedDate = invoiceValidatedEntity.UpdatedDate,
                            UpdatedBy = users.Where(u => u.Id == invoiceValidatedEntity.UpdatedById).FirstOrDefault().ForeName
                        };
                    }

                    // get comments for each sch payments
                    var filteredComments = comments.Where(c => c.LocalAuthorityId == sp.LocalAuthorityId && c.PaymentTypeId == sp.PaymentTypeId && c.ResidentId == sp.ResidentId);
                    var cmts = filteredComments.Select(c => c.Comments).ToArray();
                    sp.Comments = cmts;

                });
            });
            return new InvoiceData()
            {
                BillingCycleId = billingCycleId,
                BeginDate = billingCycle.PeriodStart,
                EndDate = billingCycle.PeriodEnd,
                BillingDate = billingCycle.BillDate,
                NumberOfDays = numOfDays,
                InvoiceResidents = invoiceDataByLa
            };
        }

        public Task<IEnumerable<BillingCycle>> GetBillingCycles()
        {
            return _invoiceDataProvider.GetBillingCycles();
        }

        public Task<bool> UpdateInvoicesValidated(InvoiceValidatedEntity[] invoiceValidatedEntities)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            var updatedById = user.Id;
            invoiceValidatedEntities.ForEach(d =>
            {
                d.ValidatedAmount = d.AmountDue;
                d.UpdatedById = updatedById;
            });

            return _invoiceDataProvider.UpdateValidatedInvoices(invoiceValidatedEntities);
        }

        public Task<bool> InsertInvoiceComments(InvoiceCommentsEntity invoiceCommentsEntity)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            var updatedById = user.Id;
            invoiceCommentsEntity.UpdatedById = updatedById;

            return _invoiceDataProvider.InsertInvoiceComments(invoiceCommentsEntity);
        }

        public Task<IEnumerable<InvoiceCommentsEntity>> GetInvoiceComments(int localAuthorityId, int billingCycleId)
        {
            return this._invoiceDataProvider.GetInvoiceComments(localAuthorityId, billingCycleId);
        }





        private IEnumerable<InvoiceResident> GetInvoiceResidentData(DateTime startDate, DateTime endDate)
        {
            // each resi may have multiple contributors, LA || LA and CC || LA and CC1, CC2 
            var schedules = this._invoiceDataProvider.GetAllSchedulesForInvoiceDate(startDate, endDate); //GetAllSchedules();
            var residents = this._residentDataProvider.GetResidentsForInvoice(startDate, endDate); //.GetAll();

            // create invoiceResident
            var invResidents = residents.Select(r =>
            {
                // find their schedules. by LA or CC or LA+CC?
                var _schedules = schedules.Where(s => s.ResidentId == r.Id);
                var _invResidents = new InvoiceResident(r.Id, $"{r.ForeName} {r.SurName}", _schedules);
                _invResidents.LocalAuthorityId = _schedules.Select(s => s.LocalAuthorityId).FirstOrDefault();
                var residentsWithCalculatedFees = _feeCalculatorService.CalculateFee(_invResidents, startDate, endDate);

                // sum LA total, LA Fee + Supliment Fee(s)
                // PaymentFrom = LA 
                var sumWeekly = residentsWithCalculatedFees.GetSchedules()
                .Where(s => s.PaymentFrom == "LA").Select(k => k.AmountDue).Sum();
                residentsWithCalculatedFees.TotalLaFee = sumWeekly;

                // get resident weekly fee (LA Fee + CC Fee)
                residentsWithCalculatedFees.ResidentWeeklyFee = residentsWithCalculatedFees.GetSchedules()
                .Where(s => s.PaymentTypeId == 1).Select(k => k.WeeklyFee).Sum(); // 1=Weekly

                // get GrandTotal (all amount dues)
                residentsWithCalculatedFees.GrandTotal = residentsWithCalculatedFees.GetSchedules()
                .Select(s => s.AmountDue).Sum();

                // order by local auth id
                residentsWithCalculatedFees.SetSchedules(
                    residentsWithCalculatedFees.GetSchedules().OrderBy(s => s.LocalAuthorityId));

                // make id zero, not visible to web client
                residentsWithCalculatedFees.Id = 0;

                return residentsWithCalculatedFees;
            }).ToArray();

            var result = invResidents.OrderBy(r => r.Name);

            return result;
        }


    }
}




/*
public Task<bool> UpdateInvoicesValidated(InvoiceData invoiceData)
{
    if (invoiceData == null) throw new ArgumentNullException(nameof(invoiceData));

    var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
    var laId = invoiceData.InvoiceResidents.FirstOrDefault().LocalAuthorityId;
    var billingCycleId = invoiceData.BillingCycleId;
    var updatedById = user.Id;

    var existingData = _invoiceDataProvider.GetValidatedInvoices((int)laId, billingCycleId).Result;
    var schedulePayments = invoiceData.InvoiceResidents.SelectMany(i => i.SchedulePayments);

    var validatedSchedulePayments = schedulePayments.Where(s => (s.Validated == "Y")).ToArray();
    List<InvoiceValidatedEntity> newRecs = new List<InvoiceValidatedEntity>();

    foreach (var s in validatedSchedulePayments)
    {
        var exists = existingData.Where(ed => ed.BillingCycleId == billingCycleId && ed.ResidentId == s.ResidentId && ed.PaymentTypeId == s.PaymentTypeId).FirstOrDefault();
        if (exists == null)
        {
            newRecs.Add(new InvoiceValidatedEntity()
            {
                LocalAuthorityId = (int)s.LocalAuthorityId,
                BillingCycleId = billingCycleId,
                ResidentId = s.ResidentId,
                PaymentTypeId = s.PaymentTypeId,
                AmountDue = s.AmountDue,
                Validated = s.Validated,
                ValidatedAmount = s.AmountDue,
                UpdatedById = updatedById
            }
            );
        }
    }

    return _invoiceDataProvider.UpdateValidatedInvoices(newRecs);
}
*/
