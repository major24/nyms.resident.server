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
    [UserAuthenticationFilter]
    public class EnquiryController : ApiController
    {
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
            var u = HttpContext.Current.User.Identity.Name;

            var enquires = _enquiryService.GetAll();

            if (enquires == null)
                return NotFound();

            return Ok(enquires.ToArray());
        }

        [HttpGet]
        [Route("api/enquires/{referenceId}")]
        public IHttpActionResult GetEnquiryByReferenceId(string referenceId)
        {
            // add validateon
            var u = HttpContext.Current.User.Identity.Name;

            var enquiry = _enquiryService.GetByReferenceId(new Guid(referenceId)).Result;

            if (enquiry == null)
                return NotFound();

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
