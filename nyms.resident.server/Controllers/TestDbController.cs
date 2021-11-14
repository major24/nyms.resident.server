using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Filters;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nyms.resident.server.Controllers
{
    [AdminAuthenticationFilter]
    public class TestDbController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IDatabaseSetupProvider _databaseSetupProvider;
        private readonly IResidentService _residentService;
        public TestDbController(IUserService userService, IDatabaseSetupProvider databaseSetupProvider, IResidentService residentService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _databaseSetupProvider = databaseSetupProvider ?? throw new ArgumentNullException(nameof(databaseSetupProvider));
            _residentService = residentService ?? throw new ArgumentNullException(nameof(residentService));
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

        [HttpPost]
        [Route("api/database/clear")]
        public HttpResponseMessage ClearDatabase()
        {
            try
            {
                _databaseSetupProvider.ClearDatabase();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
                return response;
            }
        }

        [HttpPost]
        [Route("api/database/residents/{resident}")]
        public IHttpActionResult Create([FromBody] Models.ResidentRequest resident)
        {
            // for test database
            var user = _userService.GetUsers().Where(u => u.ForeName == "Major").FirstOrDefault();
            resident.UpdatedBy = user.Id;

            _residentService.AdmitEnquiry(resident);
            return Created("", "test resident created");
        }

        [HttpPost]
        [Route("api/database/categories/clear")]
        public HttpResponseMessage ClearCategoriesDatabase()
        {
            try
            {
                _databaseSetupProvider.ClearSpendsDatabase();
                _databaseSetupProvider.ClearBudgetsDatabase();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
                return response;
            }
        }


        [HttpPost]
        [Route("api/database/spends/clear")]
        public HttpResponseMessage ClearSpendsDatabase()
        {
            try
            {
                _databaseSetupProvider.ClearSpendsDatabase();
                _databaseSetupProvider.ClearBudgetsDatabase();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
                return response;
            }
        }

        [HttpPost]
        [Route("api/database/users/clear")]
        public HttpResponseMessage ClearTestUsers()
        {
            try
            {
                _databaseSetupProvider.ClearTestUsers();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
                return response;
            }
        }


        // Meetins and agendas
        [HttpPost]
        [Route("api/database/meetings/clear")]
        public HttpResponseMessage ClearMeetingsAgendasAndActions()
        {
            try
            {
                _databaseSetupProvider.ClearMeetingsAgendasActions();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
                return response;
            }
        }


    }
}
