using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers.Reports
{
    [AdminAuthenticationFilter]
    public class ReportsController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
        }

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
