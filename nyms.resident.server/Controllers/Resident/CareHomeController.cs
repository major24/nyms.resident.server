using NLog;
using nyms.resident.server.Core;
using nyms.resident.server.Filters;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

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


        [HttpGet]
        [Route("api/enquires/{referenceId}/carehome/details")]
        public IHttpActionResult GetCareHomesDetailsByEnquiryRefId(string referenceId)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId));
            }

            var enquiry = _enquiryService.GetByReferenceId(GuidConverter.Convert(referenceId)).Result;
            if (enquiry == null)
            {
                return BadRequest("Cannot find enquiry. Please contact admin.");
            }

            var careHomeDetail = _careHomeService.GetCareHomesDetails(enquiry.CareHomeId);
            return Ok(careHomeDetail);
        }

        [HttpGet]
        [Route("api/residents/{referenceId}/carehome/details")]
        public IHttpActionResult GetCareHomesDetailsByResidentRefId(string referenceId)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId));
            }

            var resident = _residentService.GetResident(GuidConverter.Convert(referenceId));
            if (resident == null)
            {
                return BadRequest("Cannot find resident. Please contact admin.");
            }

            var careHomeDetail = _careHomeService.GetCareHomesDetails(resident.CareHomeId);
            return Ok(careHomeDetail);
        }

    }
}

