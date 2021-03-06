using Microsoft.Ajax.Utilities;
using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Http;

namespace nyms.resident.server.Controllers.Reports
{
    [AdminAuthenticationFilter]
    public class ReportsController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IReportService _reportService;
        private readonly IInvoiceService _invoiceService;

        public ReportsController(IReportService reportService, IInvoiceService invoiceService)
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        }

        // Get summary details by date
        // SAME report as in invoices ctrl, but REMOVE comments and invoiceValidateModel from SchedulePayments object
        [HttpGet]
        [Route("api/reports/invoices/summary/{startDate}/{endDate}")]
        public IHttpActionResult GetInvoicesReportByDateRange(string startDate, string endDate)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Invoice requested by {user?.ForeName}");

            if (string.IsNullOrEmpty(startDate)) throw new ArgumentNullException(nameof(startDate));
            if (string.IsNullOrEmpty(endDate)) throw new ArgumentNullException(nameof(endDate));

            DateTime.TryParse(startDate, out DateTime startDate1);
            DateTime.TryParse(endDate, out DateTime endDate1);

            if (startDate1 == null || endDate1 == null) throw new ArgumentException("Invalid dates");

            var invData = this._invoiceService.GetInvoiceData(startDate1, endDate1);

            if (invData == null)
            {
                return NotFound();
            }
            // Remove comments and invoiceValidationModes
            invData.InvoiceResidents.ForEach(r =>
            {
                if (r.SchedulePayments.Any())
                {
                    r.SchedulePayments.ForEach(sp =>
                    {
                        sp.Comments = null;
                        sp.InvoiceValidatedModel = null;
                    });
                }
            });
            return Ok(invData);
        }

        [HttpGet]
        [Route("api/reports/invoices/validations/{startDate}/{endDate}")]
        public IHttpActionResult GetValidationsReportByDateRange(string startDate, string endDate)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Invoice requested by {user?.ForeName}");

            if (string.IsNullOrEmpty(startDate)) throw new ArgumentNullException(nameof(startDate));
            if (string.IsNullOrEmpty(endDate)) throw new ArgumentNullException(nameof(endDate));

            DateTime.TryParse(startDate, out DateTime startDate1);
            DateTime.TryParse(endDate, out DateTime endDate1);

            if (startDate1 == null || endDate1 == null) throw new ArgumentException("Invalid dates");

            var invData = _invoiceService.GetValidationsInvoiceData(startDate1, endDate1);

            if (invData == null)
            {
                return NotFound();
            }
            return Ok(invData);
        }



        // Currenlty NOT used. By each individual dates
        // GET: api/Reports
        [HttpGet]
        [Route("api/reports/occupancy/{startDate}/{endDate}")]
        public IHttpActionResult GetOccupancyByDateRange(string startDate, string endDate)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Occupancy report requested by {user?.ForeName}");

            if (string.IsNullOrEmpty(startDate)) throw new ArgumentNullException(nameof(startDate));
            if (string.IsNullOrEmpty(endDate)) throw new ArgumentNullException(nameof(endDate));

            DateTime.TryParse(startDate, out DateTime startDate1);
            DateTime.TryParse(endDate, out DateTime endDate1);

            if (startDate1 == null || endDate1 == null) throw new ArgumentException("Invalid dates");

            var occupancy = this._reportService.GetOccupancyReport(startDate1, endDate1);

            if (occupancy == null)
            {
                return NotFound();
            }
            return Ok(occupancy);
        }



    }
}
