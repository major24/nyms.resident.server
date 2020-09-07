using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    // [UserAuthenticationFilter]
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

        // GET: api/Enquiry
        [Route("api/carehomes/enquires")]
        public IHttpActionResult GetAllEnquires()
        {
            var identityName = HttpContext.Current.User.Identity.Name;
            logger.Info($"All enquires requested by {identityName}");

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

            var identityName = HttpContext.Current.User.Identity.Name;
            logger.Info($"All enquires requested by {identityName}");

            var enquiry = _enquiryService.GetByReferenceId(new Guid(referenceId)).Result;

            if (enquiry == null)
            {
                logger.Warn($"No enquiry found for {referenceId}");
                return NotFound();
            }

            return Ok(enquiry);
        }

        // POST: api/Enquiry
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Enquiry/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Enquiry/5
        public void Delete(int id)
        {
        }
    }
}
