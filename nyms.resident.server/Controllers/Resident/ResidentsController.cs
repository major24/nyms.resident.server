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
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers
{
    [UserAuthenticationFilter]
    public class ResidentsController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService _userService;
        private readonly IResidentService _residentService;

        public ResidentsController(IUserService userService, IResidentService residentService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _residentService = residentService ?? throw new ArgumentNullException(nameof(residentService));
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

            return Ok(residents.ToArray());
        }

        [HttpPut]
        [Route("api/residents/{referenceId}/exit-date")]
        public IHttpActionResult UpdateExitDate(string referenceId, Models.Resident resident)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            var curUser = System.Threading.Thread.CurrentPrincipal;
            logger.Info($"Exit date updated by {user.ForeName}");

            var success = _residentService.UpdateExitDate(new Guid(referenceId), (DateTime)resident.ExitDate);

            if (!success)
            {
                return Ok(true);
            }
            logger.Warn($"Cannot update exit date");
            return Ok(false);
        }


        // POST: api/Residents
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Residents/5
        public void Put(int id, [FromBody]string value)
        {
        }

    }
}
