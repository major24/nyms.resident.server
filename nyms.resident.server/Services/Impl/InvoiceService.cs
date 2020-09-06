using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

        public IEnumerable<InvoiceResident> GetAllSchedules(DateTime billingBeginDate, DateTime billingEndDate)
        {
            // each resi may have multiple contributors, LA || LA and CC || LA and CC1, CC2 
            var schedules = this._invoiceDataProvider.GetAllSchedules();
            var residents = this._residentDataProvider.GetAll();

            // create invoiceResident
            var invResidents = residents.Select(r =>
            {
                // find their schedules. by LA or CC or LA+CC?
                var _schedules = schedules.Where(s => s.ResidentId == r.Id);
                var _invResidents = new InvoiceResident(r.Id, $"{r.ForeName} {r.SurName}", _schedules);
                var residentsWithCalculatedFees = _feeCalculatorService.CalculateFee(_invResidents, billingBeginDate, billingEndDate);

                // sum LA total, add LA fee and Supliment fees together..
                var sum = residentsWithCalculatedFees.GetSchedules().Where(s => s.PaymentFromCode == "LA").Select(k => k.AmountDue).Sum();
                residentsWithCalculatedFees.TotalLaFee = sum;

                // order by local auth id
                residentsWithCalculatedFees.SetSchedules(
                    residentsWithCalculatedFees.GetSchedules().OrderBy(s => s.LocalAuthorityId));

                // make id zero, not visible to web client
                residentsWithCalculatedFees.Id = 0;

                return residentsWithCalculatedFees;
            }).ToArray();

            return invResidents;
        }
    }
}