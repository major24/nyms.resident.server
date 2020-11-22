using NLog;
using nyms.resident.server.Core;
using nyms.resident.server.Filters;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace nyms.resident.server.Controllers
{
    [UserAuthenticationFilter]
    public class EnquiryController : ApiController
    {
        private static readonly Logger logger = Nlogger2.GetLogger();
        private readonly IEnquiryService _enquiryService;
        private readonly IResidentService _residentService;

        public EnquiryController(IUserService userService, IEnquiryService enquiryService, IResidentService residentService)
        {
            _enquiryService = enquiryService ?? throw new ArgumentNullException(nameof(enquiryService));
            _residentService = residentService ?? throw new ArgumentNullException(nameof(residentService));
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

            var enquiryListRespList = enquires.Select(e =>  e.ToEnquiryListType());
            
            return Ok(enquiryListRespList.ToArray());
        }

        [HttpGet]
        [Route("api/enquires/{referenceId}")]
        public IHttpActionResult GetEnquiryByReferenceId(string referenceId)
        {
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

            var enquiryResponse = enquiry.ToEnquiryType();
            return Ok(enquiryResponse);
        }

        // POST: api/Enquiry
        [HttpPost]
        [Route("api/carehomes/{careHomeId}/enquires")]
        public IHttpActionResult SaveEnquiry([FromUri] int careHomeId, [FromBody] Enquiry enquiry)
        {
            if (enquiry == null) return BadRequest(nameof(enquiry));
            if (enquiry.CareHomeId <= 0 || string.IsNullOrEmpty(enquiry.ForeName) || string.IsNullOrEmpty(enquiry.SurName))
                return BadRequest("Mising required fields");

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Enquiry created by {loggedInUser.ForeName}");
            enquiry.UpdatedBy = loggedInUser.Id;

            var newEnquiry = this._enquiryService.Create(enquiry).Result;
            return Created("", newEnquiry); 
        }

        // PUT: api/Enquiry/5
        [HttpPost]
        [Route("api/enquires/{referenceId}")]
        public IHttpActionResult UpdateEnquiry(string referenceId, [FromBody] Enquiry enquiry)
        {
            if (enquiry == null || string.IsNullOrEmpty(referenceId)) return BadRequest("Missing enquiry data or reference id");
            if (!GuidConverter.IsValid(enquiry.ReferenceId.ToString()))
                return BadRequest("Connot convert refence id");

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Enquiry updated by {loggedInUser.ForeName}");
            enquiry.UpdatedBy = loggedInUser.Id;

            var updEnquiry = this._enquiryService.Update(enquiry).Result;
            return Ok(updEnquiry);
        }

        // PUT: api/Enquiry/5
        [HttpPost]
        [Route("api/enquires/{referenceId}/admit")]
        public IHttpActionResult AdmitEnquiry(string referenceId, [FromBody] ResidentRequest resident)
        {
            if (resident == null || string.IsNullOrEmpty(referenceId)) return BadRequest("Missing resident data or reference id");
            if (!GuidConverter.IsValid(resident.EnquiryReferenceId.ToString()))
                return BadRequest("Connot convert enquiry refence id");
            if (resident.MoveInDate == null || resident.MoveInDate.ToString() == "")
                return BadRequest("Missing Move In Date");

            // ensure enquiry exists?
            var enqExists = _enquiryService.GetByReferenceId(resident.EnquiryReferenceId);
            if (enqExists == null) return BadRequest("Cannot locate enquiry in database. Please verify data");

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Admit an enquiry by {loggedInUser.ForeName}");
            resident.UpdatedBy = loggedInUser.Id;

            var updEnquiry = _residentService.AdmitEnquiry(resident); //this._enquiryService.Admit(resident).Result;
            return Ok(updEnquiry);
        }

        // actions
        [HttpGet]
        [Route("api/enquires/{referenceId}/actions")]
        public IHttpActionResult GetEnquiryActionsByReferenceId(string referenceId)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId));
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Enquiry actions requested by {loggedInUser.ForeName}");

            var actions = _enquiryService.GetActions(new Guid(referenceId));

            return Ok(actions);
        }

        [HttpPost]
        [Route("api/enquires/{referenceId}/actions")]
        public IHttpActionResult SaveEnquiryActions([FromUri] string referenceId, [FromBody] EnquiryAction[] enquiryActions)
        {
            if (string.IsNullOrEmpty(referenceId) || !enquiryActions.Any()) throw new ArgumentNullException("Reference Id or actions is missing");

            this._enquiryService.SaveActions(new Guid(referenceId), enquiryActions);

            return Ok(true);
        }

    }
}
