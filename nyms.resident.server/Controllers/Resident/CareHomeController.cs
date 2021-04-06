using NLog;
using nyms.resident.server.Core;
using nyms.resident.server.Filters;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Http;

namespace nyms.resident.server.Controllers
{
    [UserAuthenticationFilter]
    public class CareHomeController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly ICareHomeService _careHomeService;
        private readonly IEnquiryService _enquiryService;
        private readonly IResidentService _residentService;

        public CareHomeController(ICareHomeService careHomeService, IEnquiryService enquiryService, IResidentService residentService)
        {
            _careHomeService = careHomeService ?? throw new ArgumentNullException(nameof(careHomeService));
            _enquiryService = enquiryService ?? throw new ArgumentNullException(nameof(enquiryService));
            _residentService = residentService ?? throw new ArgumentNullException(nameof(residentService));
        }

        [HttpGet]
        [Route("api/carehomes")]
        public IHttpActionResult GetCareHomes()
        {
            var careHomes = _careHomeService.GetCareHomes();

            if (careHomes == null || !careHomes.Any())
            {
                logger.Error($"No carehome details found.");
                return NotFound();
            }

            return Ok(careHomes.ToArray());
        }

        [HttpGet]
        [Route("api/carehomes/details")]
        public IHttpActionResult GetCareHomesDetails()
        {
            var careHomes = _careHomeService.GetCareHomesDetails();

            if (careHomes == null || !careHomes.Any())
            {
                logger.Error($"No carehome details found.");
                return NotFound();
            } 

            return Ok(careHomes.ToArray());
        }
    }
}

