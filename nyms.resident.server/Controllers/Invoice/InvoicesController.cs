using NLog;
using nyms.resident.server.Invoice;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using WebGrease.Css.Extensions;

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

            var invResidents = GetAllInvoicesByDate(billingBeginDate, billingEndDate);
            
            if (invResidents == null)
            {
                // logger.Error($"No user info found for {referenceId}.");
                return NotFound();
            }
            return Ok(invResidents);
        }

        [HttpGet]
        [Route("api/invoices/all/{billingBeginDate}/{billingEndDate}/download")]
        public HttpResponseMessage DownloadAllInvoices(string billingBeginDate, string billingEndDate)
        {
            if (string.IsNullOrEmpty(billingBeginDate)) throw new ArgumentNullException(nameof(billingBeginDate));
            if (string.IsNullOrEmpty(billingEndDate)) throw new ArgumentNullException(nameof(billingEndDate));

            try
            {
                var invResidents = GetAllInvoicesByDate(billingBeginDate, billingEndDate);
                string header = "Name,Payment From,Local Authority,Description,Start Date,End Date,Num.Of Days,Weekly Fee,Amount Due," + Environment.NewLine;

                StringBuilder sb = new StringBuilder();
                string str = "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}";
                foreach (var r in invResidents)
                {
                    foreach (var s in r.Schedules)
                    {
                        sb.AppendFormat(str,
                            r.Name.Replace(",",""),
                            s.PaymentFrom,
                            s.PaymentFromName?.Replace(",", ""),
                            s.Description?.Replace(",", ""),
                            billingBeginDate,
                            billingEndDate,
                            s.NumberOfDays,
                            s.WeeklyFee.ToString().Replace(",", ""),
                            s.AmountDue.ToString().Replace(",", ""),
                            Environment.NewLine
                         );
                    }
                }

                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(header + sb.ToString());
                writer.Flush();
                stream.Position = 0;

                string fileName = String.Format("Invoice-{0}-{1}.csv", billingBeginDate, billingEndDate);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName};

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating file. " + ex.Message);
            }
            // return Ok(invResidents);
        }



        private IEnumerable<InvoiceResident> GetAllInvoicesByDate(string billingBeginDate, string billingEndDate)
        {
            DateTime billingBegin;
            DateTime billingEnd;
            DateTime.TryParse(billingBeginDate, out billingBegin);
            DateTime.TryParse(billingEndDate, out billingEnd);

            if (billingBegin == null || billingEnd == null) throw new ArgumentException("Invalid dates");

            return this._invoiceService.GetAllSchedules(billingBegin, billingEnd);
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
