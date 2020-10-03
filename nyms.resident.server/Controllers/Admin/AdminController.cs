using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers.Admin
{
    [AdminAuthenticationFilter]
    public class AdminController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPut]
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
