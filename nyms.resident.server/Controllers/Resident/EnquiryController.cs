using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebGrease.Css.Extensions;

namespace nyms.resident.server.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [UserAuthenticationFilter]
    public class EnquiryController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService _userService;
        private readonly IEnquiryService _enquiryService;

        public EnquiryController(IUserService userService, IEnquiryService enquiryService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _enquiryService = enquiryService ?? throw new ArgumentNullException(nameof(enquiryService));
        }

        [HttpGet]
        [Route("api/carehomes/{careHomeId}/enquires")]
        public IHttpActionResult GetAllEnquires()
        {
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"All enquires requested by {loggedInUser.ForeName}");

            var enquires = _enquiryService.GetAll();

            if (enquires == null)
            {
                logger.Warn($"No enquires found");
                return NotFound();
            }
            
            return Ok(enquires.ToArray());
        }

        [HttpGet]
        [Route("api/enquires/{referenceId}")]
        public IHttpActionResult GetEnquiryByReferenceId(string referenceId)
        {
            // add validateon
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId));
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Enquiry requested by {loggedInUser.ForeName}");

            var enquiry = _enquiryService.GetByReferenceId(new Guid(referenceId)).Result;

            if (enquiry == null)
            {
                logger.Warn($"No enquiry found for {referenceId}");
                return NotFound();
            }

            return Ok(enquiry);
        }

        // POST: api/Enquiry
        [HttpPost]
        [Route("api/carehomes/{careHomeId}/enquires")]
        public IHttpActionResult SaveEnquiry([FromUri] int careHomeId, [FromBody] Enquiry enquiry)
        {
            if (enquiry == null) return BadRequest(nameof(enquiry));
            if (careHomeId <= 0 && enquiry.CareCategoryId <= 0) return BadRequest("CareHomeId is required");

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Enquiry created by {loggedInUser.ForeName}");
            enquiry.UpdatedBy = loggedInUser.Id;

            var newEnquiry = this._enquiryService.Create(enquiry).Result;
            return Created("", newEnquiry); 
        }

        // PUT: api/Enquiry/5
        public void Put(int id, [FromBody]string value)
        {
        }

    }
}
