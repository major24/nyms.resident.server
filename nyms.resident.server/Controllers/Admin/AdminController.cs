using Newtonsoft.Json.Linq;
using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace nyms.resident.server.Controllers.Admin
{
    [AdminAuthenticationFilter]
    public class AdminController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        [Route("api/admin/users")]
        public IHttpActionResult CreateUser([FromBody] Models.User user)
        {
            if (user == null 
                || string.IsNullOrEmpty(user.UserName) 
                || string.IsNullOrEmpty(user.Password)
                || string.IsNullOrEmpty(user.ForeName)
                || string.IsNullOrEmpty(user.SurName))
                {
                return BadRequest("Missing required fields");
            }

            // todo validate user has role [Admin]...

            var securityPrincipal = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Creating new user by {securityPrincipal.ForeName} for {user.ForeName} {user.SurName}");

            _userService.CreateUser(user);

            return Ok("Done");
        }

        [HttpPost]
        [Route("api/admin/users/{referenceId}/roles")]
        public IHttpActionResult AssignRoles([FromUri] string referenceId, [FromBody]JObject data)  // Models.UserRolePermission[] userRolePermissions)
        {
            var userRolePermissions = data["userRolePermissions"].ToObject<Models.UserRolePermission[]>();

            if (string.IsNullOrEmpty(referenceId) || userRolePermissions == null || !userRolePermissions.Any())
            {
                return BadRequest("Missing required fields");
            }

            // todo validate user has role [Admin]...

            var user = _userService.GetByRefereneId(new Guid(referenceId)).Result;

            var securityPrincipal = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Assigning new roles to user by {securityPrincipal.ForeName} for {user.ForeName} {user.SurName}");

            _userService.AssignRoles(user.Id, userRolePermissions);

            return Ok("Done");
        }

        [HttpPost]
        [Route("api/admin/user/setpassword")]
        public IHttpActionResult SetPassword([FromBody] Models.User user)
        {

            // validate user has role [Admin]...

            // throw error if user.ref or pwd is null...
            var securityPrincipal = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Reseting password by {securityPrincipal.ForeName} for {user.ReferenceId}");

            _userService.SetPassword(user.ReferenceId, user.Password);

            return Ok("Done");
        }


   }
}
