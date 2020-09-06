using NLog;
using nyms.resident.server.Invoice;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers.Invoice
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class InvoicesController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        }

        // GET: api/Invoices
        [HttpGet]
        [Route("api/invoices/all/{billingBeginDate}/{billingEndDate}")]
        public IHttpActionResult GetAllInvoices(string billingBeginDate, string billingEndDate)
        {
            if (string.IsNullOrEmpty(billingBeginDate)) throw new ArgumentNullException(nameof(billingBeginDate));
            if (string.IsNullOrEmpty(billingEndDate)) throw new ArgumentNullException(nameof(billingEndDate));

            DateTime billingBegin;
            DateTime billingEnd;
            DateTime.TryParse(billingBeginDate, out billingBegin);
            DateTime.TryParse(billingEndDate, out billingEnd);

            if (billingBegin == null || billingEnd == null) throw new ArgumentException("Invalid dates");

            var invResidents = this._invoiceService.GetAllSchedules(billingBegin, billingEnd);
            if (invResidents == null)
            {
                // logger.Error($"No user info found for {referenceId}.");
                return NotFound();
            }

            return Ok(invResidents);
        }

        // GET: api/Invoices/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Invoices
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Invoices/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Invoices/5
        public void Delete(int id)
        {
        }
    }
}
