using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers.Resident
{
    [AdminAuthenticationFilter]
    public class ScheduleController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IScheduleService _scheduleService;
        private readonly IResidentService _residentService;

        public ScheduleController(IScheduleService scheduleService, IResidentService residentService)
        {
            _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
            _residentService = residentService ?? throw new ArgumentNullException(nameof(residentService));
        }

        [HttpGet]
        [Route("api/schedules/payment-providers")]
        public IHttpActionResult GetPaymentProviders()
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Payment providers requested by {user?.ForeName}");

            var paymentProviders = this._scheduleService.GetPaymentProviders();
            if (paymentProviders == null) return NotFound();

            return Ok(paymentProviders);
        }

        [HttpGet]
        [Route("api/schedules/payment-types")]
        public IHttpActionResult GetPaymentTyps()
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Payment providers requested by {user?.ForeName}");

            var paymentTypes = this._scheduleService.GetPaymentTypes();
            if (paymentTypes == null) return NotFound();

            return Ok(paymentTypes);
        }

    }
}
