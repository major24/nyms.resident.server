using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nyms.resident.server.Controllers
{
    public class TestDbController : ApiController
    {
        private readonly IUserService _userService;

        public TestDbController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        [Route("api/testdb")]
        public HttpResponseMessage TestDbConnection()
        {
            try
            {
                var result = this._userService.GetUsers();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            }
            catch(Exception ex)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
                return response;
            }
        }
    }
}
