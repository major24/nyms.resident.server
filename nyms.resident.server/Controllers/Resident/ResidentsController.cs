﻿using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace nyms.resident.server.Controllers
{
    [UserAuthenticationFilter]
    public class ResidentsController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IResidentService _residentService;
        private readonly IScheduleService _scheduleService;

        public ResidentsController(IUserService userService, IResidentService residentService, IScheduleService scheduleService)
        {
            _residentService = residentService ?? throw new ArgumentNullException(nameof(residentService));
            _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
        }

        [HttpGet]
        [Route("api/carehomes/{careHomeId}/residents/active")]
        public IHttpActionResult GetAllResidents(int careHomeId)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            var curUser = System.Threading.Thread.CurrentPrincipal;
            logger.Info($"All resedents requested by {user.ForeName}");

            var residents = _residentService.GetResidentsByCareHomeId(careHomeId);

            if (residents == null)
            {
                logger.Warn($"No residents found");
                return NotFound();
            }

            var residentResponseList = residents.Select(r => r.ToResidentListType());

            return Ok(residentResponseList.ToArray());
        }

        [HttpPost]
        [Route("api/residents/{referenceId}/exit-date")]
        public IHttpActionResult UpdateExitDate(string referenceId, Models.Resident resident)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Exit date updated by {user.ForeName}");

            var success = _residentService.UpdateExitDate(new Guid(referenceId), (DateTime)resident.ExitDate);

            if (!success)
            {
                return Ok(true);
            }
            logger.Warn($"Cannot update exit date");
            return Ok(false);
        }


        [HttpGet]
        [Route("api/residents/{referenceId}")]
        public IHttpActionResult GetResidentByReferenceId(string referenceId)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId));
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Enquiry requested by {loggedInUser.ForeName}");

            var resident = _residentService.GetResident(new Guid(referenceId));

            if (resident == null)
            {
                logger.Warn($"No resident found for {referenceId}");
                return NotFound();
            }
            var result = resident.ToResidentResponseType();
            return Ok(result);
        }



        // *** Schedules ***
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

        [HttpPost]
        [Route("api/residents/schedules/{id}/end-date")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]ScheduleEndDateEntity scheduleEndDateEntity)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Schedule updated by {user?.ForeName}");

            this._scheduleService.UpdateScheduleEndDate(id, scheduleEndDateEntity.ScheduleEndDate);

            return Ok("Updated");
        }

        [HttpPost]
        [Route("api/residents/schedules/{id}/inactivate")]
        public IHttpActionResult InactivteSchedule([FromUri]int id)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Schedule updated by {user?.ForeName}");

            this._scheduleService.InactivateSchedule(id);

            return Ok("Updated");
        }

    }
}
