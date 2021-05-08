using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nyms.resident.server.Controllers.Security
{
    [UserAuthenticationFilter]
    public class SecurityController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly ISecurityService _securityService;

        public SecurityController(ISecurityService securityService)
        {
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        }

        [HttpGet]
        [Route("api/security/roles")]
        public IHttpActionResult GetRoles()
        {
            var roles = _securityService.GetRoles();
            if (roles == null)
            {
                logger.Error($"No roles found");
                return NotFound();
            }

            return Ok(roles);
        }

    }
}
