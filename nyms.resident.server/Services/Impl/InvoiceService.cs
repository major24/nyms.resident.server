using Microsoft.Ajax.Utilities;
using NLog;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Impl
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceDataProvider _invoiceDataProvider;
        private readonly IResidentDataProvider _residentDataProvider;
        private readonly IFeeCalculatorService _feeCalculatorService;
        private readonly IUserService _userService;
        private readonly IBillingCycleDataProvider _billingCycleDataProvider;

        public InvoiceService(IInvoiceDataProvider invoiceDataProvider, 
            IResidentDataProvider residentDataProvider, 
            IFeeCalculatorService feeCalculatorService, 
            IUserService userService,
            IBillingCycleDataProvider billingCycleDataProvider)
        {
            _invoiceDataProvider = invoiceDataProvider ?? throw new ArgumentNullException(nameof(invoiceDataProvider));
            _residentDataProvider = residentDataProvider ?? throw new ArgumentNullException(nameof(residentDataProvider));
            _feeCalculatorService = feeCalculatorService ?? throw new ArgumentNullException(nameof(feeCalculatorService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _billingCycleDataProvider = billingCycleDataProvider ?? throw new ArgumentNullException(nameof(billingCycleDataProvider));
        }

        // Data by Date Range
        public InvoiceData GetInvoiceData(DateTime startDate, DateTime endDate)
        {
            return GetCalucatedInvoiceData(startDate, endDate);
        }

        public InvoiceData GetInvoiceData(int billingCycleId)
        {
            var billingCycles = GetBillingCycles().Result;
            var billingCycle = billingCycles.Where(bc => bc.Id == billingCycleId).FirstOrDefault();
            if (billingCycle == null) throw new ArgumentNullException(nameof(billingCycle));

            return GetCalucatedInvoiceData(billingCycle.PeriodStart, billingCycle.PeriodEnd, billingCycle.LocalAuthorityId, billingCycleId);
        }

        public IEnumerable<InvoiceValidationsReportResponse> GetValidationsInvoiceData(DateTime startDate, DateTime endDate)
        {
            // get list of billing cycle ids based on start and end date
            // each billing ids, get report (use above fn)
            // create a new list of response based on the data above (by each bill id)
            var billingCycles = _billingCycleDataProvider
                .GetBillingCycles().Result
                .Where(bc => bc.PeriodStart >= startDate && bc.PeriodEnd <= endDate);
                
            if (billingCycles == null) throw new ArgumentNullException(nameof(billingCycles));

            List<InvoiceValidationsReportResponse> result = new List<InvoiceValidationsReportResponse>();

            billingCycles.ForEach((bc) =>
            {
                InvoiceData invData = GetCalucatedInvoiceData(bc.PeriodStart, bc.PeriodEnd, bc.LocalAuthorityId, bc.Id);  // GetInvoiceData(bc.Id);
                if (invData != null)
                {
                    if (invData.InvoiceResidents.Any())
                    {
                        invData.InvoiceResidents.ForEach((iv) =>
                        {
                            if (iv.SchedulePayments.Any())
                            {
                                iv.SchedulePayments.ForEach((sp) =>
                                {
                                    InvoiceValidationsReportResponse invRpt = new InvoiceValidationsReportResponse();
                                    invRpt.BillingCycleId = bc.Id;
                                    invRpt.PeriodStartDate = invData.BeginDate;
                                    invRpt.PeriodEndDate = invData.EndDate;
                                    invRpt.Name = iv.Name;

                                    invRpt.LocalAuthorityId = sp.LocalAuthorityId;
                                    invRpt.LocalAuthority = sp.PaymentFromName;
                                    invRpt.PaymentTypeId = sp.PaymentTypeId;
                                    invRpt.Description = sp.Description;
                                    invRpt.ResidentId = sp.ResidentId;
                                    invRpt.AmountDue = sp.AmountDue;
                                    invRpt.ValidatedAmount = sp.InvoiceValidatedModel.ValidatedAmount;
                                    invRpt.Validated = sp.InvoiceValidatedModel.Validated;
                                    invRpt.UpdatedBy = sp.InvoiceValidatedModel.UpdatedBy;
                                    invRpt.UpdatedDate = sp.InvoiceValidatedModel.UpdatedDate;

                                    result.Add(invRpt);
                                });
                            }
                        }); // invData foreach
                    }
                }
            }); // billing cycle foreach

            return result;
        }


        private InvoiceData GetCalucatedInvoiceData(DateTime startDate, DateTime endDate, int localAuthorityId = 0, int billingCycleId = 0)
        {
            if (startDate == null) throw new ArgumentNullException(nameof(startDate));
            if (endDate == null) throw new ArgumentNullException(nameof(endDate));

            var invoiceResidents = this.GetInvoiceResidentData(startDate, endDate);
            if (invoiceResidents == null || !invoiceResidents.Any()) return null;

            if (localAuthorityId > 0)
            {
                invoiceResidents = invoiceResidents.Where(d => d.LocalAuthorityId == localAuthorityId);
            }
            
            var numOfDays = _feeCalculatorService.GetNumberOfDaysInMonth(startDate, endDate); 
            // get usernames for mapping
            var users = _userService.GetUsers();

            // assemble validated date with invoice data
            var validatedInvoiceData = _invoiceDataProvider.GetValidatedInvoices(startDate, endDate).Result; 
          
            // get comments
            var comments = _invoiceDataProvider.GetInvoiceComments(startDate, endDate).Result; 

            invoiceResidents.ForEach(d =>
            {
                d.SchedulePayments.ForEach(sp =>
                {
                    var invoiceValidatedEntity = validatedInvoiceData.Where(ed =>
                        ed.LocalAuthorityId == sp.LocalAuthorityId &&
                        ed.PaymentTypeId == sp.PaymentTypeId &&
                        ed.ResidentId == sp.ResidentId &&
                        ed.ScheduleId == sp.Id).FirstOrDefault();
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
                    var filteredComments = comments
                        .Where(c => c.LocalAuthorityId == sp.LocalAuthorityId && c.PaymentTypeId == sp.PaymentTypeId && c.ResidentId == sp.ResidentId);
                    var cmts = filteredComments.Select(c => c.Comments).ToArray();
                    sp.Comments = cmts;

                });
            });
            return new InvoiceData()
            {
                BillingCycleId = billingCycleId,
                BeginDate = startDate,
                EndDate = endDate,
                BillingDate = DateTime.Now, //Todo
                NumberOfDays = numOfDays,
                InvoiceResidents = invoiceResidents
            };
        }

        public Task<IEnumerable<BillingCycle>> GetBillingCycles()
        {
            return _billingCycleDataProvider.GetBillingCycles();
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

        // CORE function
        private IEnumerable<InvoiceResident> GetInvoiceResidentData(DateTime startDate, DateTime endDate)
        {
            // each resi may have multiple contributors, LA || LA and CC || LA and CC1, CC2 
            var schedules = this._invoiceDataProvider.GetAllSchedulesForInvoiceDate(startDate, endDate);
            var residents = this._residentDataProvider
                .GetResidents()
                .Where(res => res.ExitDate >= startDate && res.AdmissionDate <= endDate);

            // create invoiceResident
            var invResidents = residents.Select(r =>
            {
                // find their schedules. by LA or CC or LA+CC?
                var _schedules = schedules.Where(s => s.ResidentId == r.Id);
                var _invResident = new InvoiceResident(r.Id, $"{r.ForeName} {r.SurName}", _schedules);
                _invResident.LocalAuthorityId = _schedules.Select(s => s.LocalAuthorityId).FirstOrDefault();
                // logger.Info($"Calculate invoice amount {_invResident.Name}");
                var residentsWithCalculatedFees = _feeCalculatorService.CalculateFee(_invResident, startDate, endDate);

                // sum LA total, LA Fee + Supliment Fee(s)
                // PaymentProviderId = LA=1, CC=2, PV=3
                var sumWeekly = residentsWithCalculatedFees.GetSchedules()
                .Where(s => s.PaymentProviderId == 1).Select(k => k.AmountDue).Sum();
                residentsWithCalculatedFees.TotalLaFee = sumWeekly;

                // get resident weekly fee (LA Fee + CC Fee)
                var payTypeIds = residentsWithCalculatedFees.GetSchedules().Select(s => s.PaymentTypeId).Distinct();
                payTypeIds.ForEach(payId => 
                {
                    if (payId <= 3) // paymentTypeId 1 = La, 2 = CC 3 = Private
                    {
                        var wf = residentsWithCalculatedFees.GetSchedules().Where(s => s.PaymentTypeId == payId).Select(s => s.WeeklyFee).FirstOrDefault();
                        residentsWithCalculatedFees.ResidentWeeklyFee += wf;
                    }
                });

                // get GrandTotal (all amount dues)
                residentsWithCalculatedFees.GrandTotal = residentsWithCalculatedFees.GetSchedules()
                .Select(s => s.AmountDue).Sum();

                // order by local auth id
                residentsWithCalculatedFees.SetSchedules(
                    residentsWithCalculatedFees.GetSchedules().OrderBy(s => s.LocalAuthorityId));

                // Added la div names for report purposes
                residentsWithCalculatedFees.LocalAuthorityName = r.LocalAuthorityName;
                residentsWithCalculatedFees.CareHomeDivisionId = r.CareHomeDivisionId;
                residentsWithCalculatedFees.CareHomeDivisionName = r.CareHomeDivisionName;

                return residentsWithCalculatedFees;
            }).ToArray();

            var result = invResidents.OrderBy(r => r.Name);

            return result;
        }

    }
}

