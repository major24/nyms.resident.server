using nyms.resident.server.Filters;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers.User
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [UserAuthenticationFilter]
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        [Route("api/users/{referenceId}")]
        public IHttpActionResult GetUserByReferenceId(string referenceId)
        {
            if (referenceId == null || referenceId == "")
            {
                return BadRequest($"Must provide a user reference id");
            }

            Guid _referenceId = new Guid(referenceId);
            var user = _userService.GetByRefereneId(_referenceId).Result;
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [Route("api/users/carehomeusers/{referenceId}")]
        public IHttpActionResult GetCareHomeUserByReferenceId(string referenceId)
        {
            if (referenceId == null || referenceId == "")
            {
                return BadRequest($"Must provide a user reference id");
            }

            Guid _referenceId = new Guid(referenceId);
            var user = _userService.GetCareHomeUser(_referenceId).Result;
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [Route("api/users/{referenceId}/roles")]
        public IHttpActionResult GetByUserAndRolesByReferenceId(string referenceId)
        {
            if (referenceId == null || referenceId == "")
            {
                return BadRequest($"Must provide a user reference id");
            }

            Guid _referenceId = new Guid(referenceId);
            var user = new Models.User() { };
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // PUT: api/Users/5
        // [HttpPut("{referenceId}")]
        [HttpPut]
        [Route("api/users/{referenceId}")]
        public void Put(string referenceId, [FromBody] Models.User user)
        {
            /*this._userService.UpdateUser(user);*/
        }

    }
}