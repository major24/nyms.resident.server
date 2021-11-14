using Microsoft.Ajax.Utilities;
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
using System.Web;
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
            var userRoles = _securityService.GetUserRoleAccesses();
            if (userRoles == null || !userRoles.Any())
            {
                logger.Error($"No user roles found");
                return NotFound();
            }

            var roles = userRoles.DistinctBy(ur => new { ur.RoleId })
            .Select(urd => {
                return new Role
                {
                    Id = urd.RoleId,
                    Name = urd.RoleName
                };
            }).ToArray();

            return Ok(roles);
        }

        [HttpGet]
        [Route("api/security/users/roles/{roleId}")]
        public IHttpActionResult GetUserRolesByRoleId(int roleId)
        {
            var userRoles = _securityService.GetUserRoleAccesses();
            if (userRoles == null || !userRoles.Any())
            {
                logger.Error($"No user roles found");
                return NotFound();
            }

            var selUserRoles = userRoles.Where(r => r.RoleId == roleId).Select(u => {
                return new UserRoleAccess()
                {
                    UserId = u.UserId,
                    ReferenceId = u.ReferenceId,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    RoleId = u.RoleId,
                    RoleName = u.RoleName,
                    CareHomeId = u.CareHomeId
                };
            }).ToArray();

            return Ok(selUserRoles);
        }

        [HttpGet]
        [Route("api/security/users/roles/{roleId}/hasaccess")]
        public IHttpActionResult HasAccessToRole(int roleId)
        {
            var userRoles = _securityService.GetUserRoleAccesses();
            if (userRoles == null || !userRoles.Any())
            {
                logger.Error($"No user roles found");
                return NotFound();
            }

            var user = HttpContext.Current.User as SecurityPrincipal;
            if (user == null)
            {
                logger.Error($"No user roles found");
                throw new ArgumentNullException("user not found");
            }

            var hasAccess = userRoles.Where(r => r.RoleId == roleId && r.UserId == user.Id).FirstOrDefault();

            return Ok(hasAccess != null ? true : false);
        }

    }
}
