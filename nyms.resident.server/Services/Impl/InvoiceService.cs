using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
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

        public InvoiceService(IInvoiceDataProvider invoiceDataProvider, IResidentDataProvider residentDataProvider, IFeeCalculatorService feeCalculatorService)
        {
            _invoiceDataProvider = invoiceDataProvider ?? throw new ArgumentException(nameof(invoiceDataProvider));
            _residentDataProvider = residentDataProvider ?? throw new ArgumentException(nameof(residentDataProvider));
            _feeCalculatorService = feeCalculatorService ?? throw new ArgumentNullException(nameof(feeCalculatorService));
        }

        public IEnumerable<InvoiceResident> GetInvoiceData(DateTime billingBeginDate, DateTime billingEndDate)
        {
            // each resi may have multiple contributors, LA || LA and CC || LA and CC1, CC2 
            var schedules = this._invoiceDataProvider.GetAllSchedulesForInvoiceDate(billingBeginDate, billingEndDate); //GetAllSchedules();
            var residents = this._residentDataProvider.GetResidentsForInvoice(billingBeginDate, billingEndDate); //.GetAll();

            // create invoiceResident
            var invResidents = residents.Select(r =>
            {
                // find their schedules. by LA or CC or LA+CC?
                var _schedules = schedules.Where(s => s.ResidentId == r.Id);
                var _invResidents = new InvoiceResident(r.Id, $"{r.ForeName} {r.SurName}", _schedules);
                _invResidents.LocalAuthorityId = _schedules.Select(s => s.LocalAuthorityId).FirstOrDefault();
                var residentsWithCalculatedFees = _feeCalculatorService.CalculateFee(_invResidents, billingBeginDate, billingEndDate);

                // sum LA total, LA Fee + Supliment Fee(s)
                // PaymentFrom = LA 
                var sumWeekly = residentsWithCalculatedFees.GetSchedules()
                .Where(s => s.PaymentFrom == "LA").Select(k => k.AmountDue).Sum();
                residentsWithCalculatedFees.TotalLaFee = sumWeekly;

                // get resident weekly fee (LA Fee + CC Fee)
                residentsWithCalculatedFees.ResidentWeeklyFee = residentsWithCalculatedFees.GetSchedules()
                .Where(s => s.PaymentType == "WEEKLY").Select(k => k.WeeklyFee).Sum();

                // get GrandTotal (all amount dues)
                residentsWithCalculatedFees.GrandTotal =  residentsWithCalculatedFees.GetSchedules()
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


        public IEnumerable<InvoiceResident> GetInvoiceData(int localAuthorityId, int billingCycleId)
        {
            var billingCycles = this.GetBillingCycles().Result;
            var billingCycle = billingCycles.Where(bc => bc.Id == billingCycleId).FirstOrDefault();
            if (billingCycle == null) throw new ArgumentNullException(nameof(billingCycle));

            var invoiceData = this.GetInvoiceData(billingCycle.PeriodStart, billingCycle.PeriodEnd);
            var invoiceDataByLa = invoiceData.Where(d => d.LocalAuthorityId == localAuthorityId);

            return invoiceDataByLa;
        }

        public Task<IEnumerable<BillingCycle>> GetBillingCycles()
        {
            return _invoiceDataProvider.GetBillingCycles();
        }
    }
}

