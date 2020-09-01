using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class AuthenticationController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _authenticationService = authenticationService; // add throw error null??
        }

        [HttpPost]
        [Route("api/authentication/authenticate")]
        public IHttpActionResult Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            var response = _authenticationService.Authenticate(authenticationRequest);
            if (response == null)
                return BadRequest("Username or password is incorrect");

            return Ok(response);
        }

    }
}
