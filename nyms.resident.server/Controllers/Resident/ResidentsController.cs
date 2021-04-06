using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using nyms.resident.server.Core;

namespace nyms.resident.server.Controllers
{
    [UserAuthenticationFilter]
    public class ResidentsController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IResidentService _residentService;
        private readonly IScheduleService _scheduleService;
        private readonly ICareHomeService _careHomeService;

        public ResidentsController(IUserService userService, IResidentService residentService, IScheduleService scheduleService, ICareHomeService careHomeService)
        {
            _residentService = residentService ?? throw new ArgumentNullException(nameof(residentService));
            _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
            _careHomeService = careHomeService ?? throw new ArgumentNullException(nameof(careHomeService));
        }

        [HttpGet]
        [Route("api/carehomes/{careHomeId}/residents")]
        public IHttpActionResult GetAllResidentsAll(int careHomeId)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            var curUser = System.Threading.Thread.CurrentPrincipal;
            logger.Info($"All resedents requested by {user.ForeName}");

            var residents = _residentService.GetAllResidentsByCareHomeId(careHomeId);

            if (residents == null)
            {
                logger.Warn($"No residents found");
                return NotFound();
            }

            var residentResponseList = residents.Select(r => r.ToResidentListType());

            return Ok(residentResponseList.ToArray());
        }

        [HttpGet]
        [Route("api/carehomes/{careHomeId}/residents/active")]
        public IHttpActionResult GetAllResidents(int careHomeId)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            var curUser = System.Threading.Thread.CurrentPrincipal;
            logger.Info($"All resedents requested by {user.ForeName}");

            var residents = _residentService.GetActiveResidentsByCareHomeId(careHomeId);

            if (residents == null)
            {
                logger.Warn($"No residents found");
                return NotFound();
            }

            var residentResponseList = residents.Select(r => r.ToResidentListType());

            return Ok(residentResponseList.ToArray());
        }

        [HttpPost]
        [Route("api/residents/{referenceId}/discharge")]
        public IHttpActionResult DischargeResident(string referenceId, ResidentDischargeExitRequest residentDischargeExitRequest)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Exit date updated by {user.ForeName}");

            var success = _residentService.DischargeResident(new Guid(referenceId), residentDischargeExitRequest.DischargedFromHomeDate);

            if (!success)
            {
                return Ok(true);
            }
            logger.Warn($"Cannot update exit date");
            return Ok(false);
        }

        [HttpPost]
        [Route("api/residents/{referenceId}/exitschedule")]
        public IHttpActionResult ExitInvoiceResident(string referenceId, ResidentDischargeExitRequest residentDischargeExitRequest)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Exit date updated by {user.ForeName}");

            var success = _residentService.ExitInvoiceResident(new Guid(referenceId), (DateTime)residentDischargeExitRequest.ExitDate);

            if (!success)
            {
                return Ok(true);
            }
            logger.Warn($"Cannot update exit date");
            return Ok(false);
        }

        [HttpPost]
        [Route("api/residents/{referenceId}/activate")]
        public IHttpActionResult ActivateResident(string referenceId)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Exit date updated by {user.ForeName}");

            var success = _residentService.ActivateResident(new Guid(referenceId));

            if (!success)
            {
                return Ok(true);
            }
            logger.Warn($"Cannot activate resident");
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

        [HttpPost]
        [Route("api/residents/{referenceId}")]
        public IHttpActionResult UpdateResident(string referenceId, ResidentRequest resident)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId));
            }
            if (resident == null)
            {
                throw new ArgumentNullException(nameof(resident));
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Resident updated by {loggedInUser.ForeName}");
            resident.UpdatedBy = loggedInUser.Id;

            var result = _residentService.Update(resident);
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
            if (schedule.LocalAuthorityId == 0)
            {
                schedule.LocalAuthorityId = resident.LocalAuthorityId;
            }
            
            if (schedule.Id > 0)
            {
                _scheduleService.UpdateSchedule(schedule);
            } 
            else
            {
                this._scheduleService.CreateSchedule(schedule);
            }
           

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

        [HttpGet]
        [Route("api/residents/{referenceId}/carehome/details")]
        public IHttpActionResult GetCareHomesDetailsByResidentRefId(string referenceId)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId));
            }

            var resident = _residentService.GetResident(GuidConverter.Convert(referenceId));
            if (resident == null)
            {
                return BadRequest("Cannot find resident. Please contact admin.");
            }

            var careHomeDetail = _careHomeService.GetCareHomesDetails(resident.CareHomeId);
            return Ok(careHomeDetail);
        }

    }
}
