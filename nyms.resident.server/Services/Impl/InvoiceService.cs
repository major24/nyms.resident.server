using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models.Authentication;
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
            return GetCalucatedInvoiceData(startDate, endDate);
        }

        public InvoiceData GetInvoiceData(int localAuthorityId, int billingCycleId)
        {
            var billingCycles = GetBillingCycles().Result;
            var billingCycle = billingCycles.Where(bc => bc.Id == billingCycleId).FirstOrDefault();
            if (billingCycle == null) throw new ArgumentNullException(nameof(billingCycle));

            return GetCalucatedInvoiceData(billingCycle.PeriodStart, billingCycle.PeriodEnd, localAuthorityId, billingCycleId);
        }

        private InvoiceData GetCalucatedInvoiceData(DateTime startDate, DateTime endDate, int localAuthorityId = 0, int billingCycleId = 0)
        {
            if (startDate == null) throw new ArgumentNullException(nameof(startDate));
            if (endDate == null) throw new ArgumentNullException(nameof(endDate));

            var invoiceResidents = this.GetInvoiceResidentData(startDate, endDate);
            if (invoiceResidents == null || !invoiceResidents.Any()) return null;
            // also return if schedules is null as well
            // if (!invoiceResidents.FirstOrDefault().SchedulePayments.Any()) return null;

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
                    var filteredComments = comments.Where(c => c.LocalAuthorityId == sp.LocalAuthorityId && c.PaymentTypeId == sp.PaymentTypeId && c.ResidentId == sp.ResidentId);
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

        // CORE function
        private IEnumerable<InvoiceResident> GetInvoiceResidentData(DateTime startDate, DateTime endDate)
        {
            // each resi may have multiple contributors, LA || LA and CC || LA and CC1, CC2 
            var schedules = this._invoiceDataProvider.GetAllSchedulesForInvoiceDate(startDate, endDate);
            var residents = this._residentDataProvider.GetResidentsForInvoice(startDate, endDate);

            // create invoiceResident
            var invResidents = residents.Select(r =>
            {
                // find their schedules. by LA or CC or LA+CC?
                var _schedules = schedules.Where(s => s.ResidentId == r.Id);
                var _invResidents = new InvoiceResident(r.Id, $"{r.ForeName} {r.SurName}", _schedules);
                _invResidents.LocalAuthorityId = _schedules.Select(s => s.LocalAuthorityId).FirstOrDefault();
                var residentsWithCalculatedFees = _feeCalculatorService.CalculateFee(_invResidents, startDate, endDate);

                // sum LA total, LA Fee + Supliment Fee(s)
                // PaymentProviderId = LA=1, CC=2, PV=3
                var sumWeekly = residentsWithCalculatedFees.GetSchedules()
                .Where(s => s.PaymentProviderId == 1).Select(k => k.AmountDue).Sum();
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
                // residentsWithCalculatedFees.Id = 0;

                return residentsWithCalculatedFees;
            }).ToArray();

            var result = invResidents.OrderBy(r => r.Name);

            return result;
        }


    }
}

