using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
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
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IScheduleService _scheduleService;
        private readonly IResidentService _residentService;

        public ScheduleController(IScheduleService scheduleService, IResidentService residentService)
        {
            _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
            _residentService = residentService ?? throw new ArgumentNullException(nameof(residentService));
        }

        [HttpGet]
        [Route("api/residents/schedules")]
        public IHttpActionResult GetResidentsSchedules()
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Schedules requested by {user?.ForeName}");

            var schedules = this._scheduleService.GetResidentSchedules();
            if (schedules == null) return NotFound();

            return Ok(schedules);
        }

        [HttpGet]
        [Route("api/residents/{referenceId}/schedules")]
        public IHttpActionResult GetResidentsSchedules(string referenceId)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Schedules requested by {user?.ForeName}");

            var schedules = this._scheduleService.GetResidentSchedules(new Guid(referenceId));
            if (schedules == null) return NotFound();

            return Ok(schedules);
        }

        [HttpPost]
        [Route("api/residents/{referenceId}/schedules")]
        public IHttpActionResult Post([FromUri]string referenceId, [FromBody]ScheduleEntity schedule)
        {
            if (string.IsNullOrEmpty(referenceId)) return BadRequest(referenceId);

            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Schedule created by {user?.ForeName} for {schedule.ResidentId}");

            var resident = this._residentService.GetResident(new Guid(referenceId));
            if (resident == null)
            {
                return BadRequest("Resident not found");
            }
            schedule.ResidentId = resident.Id;
            schedule.LocalAuthorityId = resident.LocalAuthorityId;

            this._scheduleService.CreateSchedule(schedule);

            return Created("", "Saved");
        }

        [HttpPut]
        [Route("api/residents/schedules/{id}/end-date")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]ScheduleEndDateEntity scheduleEndDateEntity)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Schedule updated by {user?.ForeName}");

            this._scheduleService.UpdateScheduleEndDate(id, scheduleEndDateEntity.ScheduleEndDate);

            return Ok("Updated");
        }

        [HttpPut]
        [Route("api/residents/schedules/{id}/inactivate")]
        public IHttpActionResult InactivteSchedule([FromUri]int id)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Schedule updated by {user?.ForeName}");

            this._scheduleService.InactivateSchedule(id);

            return Ok("Updated");
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
