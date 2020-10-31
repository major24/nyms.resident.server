﻿using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers
{
    [UserAuthenticationFilter]
    public class CareHomeController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly ICareHomeService _careHomeService;

        public CareHomeController(ICareHomeService careHomeService)
        {
            _careHomeService = careHomeService ?? throw new ArgumentNullException(nameof(careHomeService));
        }

        [HttpGet]
        [Route("api/carehomes/details")]
        public IHttpActionResult GetCareHomesDetails()   // IEnumerable<CareHome> 
        {
            var careHomes = _careHomeService.GetCareHomesDetails();

            if (careHomes == null)
            {
                logger.Error($"No carehome details found.");
                return NotFound();
            } 

            return Ok(careHomes.ToArray());
        }


    }
}






/*        [HttpGet("api/carehomes/{referenceId}/details")]
        public ActionResult<CareHome> GetByUserAndRolesByReferenceId(string referenceId)
        {
            if (referenceId == null || referenceId == "")
            {
                return BadRequest($"Must provide a user reference id");
            }

            Guid _referenceId = new Guid(referenceId);
            var careHome = _careHomeService.GetCareHomeByReferenceId(_referenceId);

            if (careHome == null)
                return NotFound($"CareHome not found for the provided id {referenceId}");

            return careHome;
        }*/
