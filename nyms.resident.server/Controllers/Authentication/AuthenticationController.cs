using NLog;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers
{
    public class AuthenticationController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost]
        [Route("api/authentication/authenticate")]
        public IHttpActionResult Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            logger.Info($"Authentication requested for {authenticationRequest.UserName}");

            var response = _authenticationService.Authenticate(authenticationRequest);
            if (response == null)
            {
                logger.Warn($"Authentication failed for {authenticationRequest.UserName}");
                return BadRequest("Username or password is incorrect");
            }
                
            logger.Info($"Authentication successful for {authenticationRequest.UserName}");
            return Ok(response);
        }

    }
}
