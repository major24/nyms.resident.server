using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace nyms.resident.server.Controllers.Invoice
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [AdminAuthenticationFilter]
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
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Invoice requested by {user?.ForeName}");

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
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Invoice requested by {user?.ForeName}");

            if (string.IsNullOrEmpty(billingBeginDate)) throw new ArgumentNullException(nameof(billingBeginDate));
            if (string.IsNullOrEmpty(billingEndDate)) throw new ArgumentNullException(nameof(billingEndDate));

            try
            {
                var invResidents = GetAllInvoicesByDate(billingBeginDate, billingEndDate);

                var detailCsvReport = CreateDetailCsvReport(invResidents, billingBeginDate, billingEndDate);
                var singleLineCsvReport = CreateSingleLineCsvReport(invResidents, billingBeginDate, billingEndDate);

                StringBuilder sb = new StringBuilder();
                sb.Append(detailCsvReport)
                    .Append(Environment.NewLine).Append(Environment.NewLine)
                    .Append(singleLineCsvReport);


                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(sb.ToString());
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
        }

        private string CreateDetailCsvReport(IEnumerable<InvoiceResident> invResidents, string billingBeginDate, string billingEndDate)
        {
            string header = "Name,Payment From,Local Authority,Description,Start Date,End Date,Num.Of Days,Weekly Fee,Amount Due," + Environment.NewLine;

            StringBuilder sb = new StringBuilder();
            string str = "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}";
            foreach (var r in invResidents)
            {
                foreach (var s in r.Schedules)
                {
                    sb.AppendFormat(str,
                        r.Name.Replace(",", ""),
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
            return header + sb.ToString();
        }

        private string CreateSingleLineCsvReport(IEnumerable<InvoiceResident> invResidents, string billingBeginDate, string billingEndDate)
        {
            string header = "Name,Start Date,End Date,Num.Of Days,LA,CC,PV," + Environment.NewLine;

            StringBuilder sb = new StringBuilder();
            string str = "{0},{1},{2},{3},{4},{5},{6},{7}";
            foreach (var r in invResidents)
            {
                var laAmt = GetTotal(r.Schedules, "LA");
                var ccAmt = GetTotal(r.Schedules, "CC");
                var pvAmt = GetTotal(r.Schedules, "PV");
                var numOfDays = r.Schedules.Select(s => s.NumberOfDays).FirstOrDefault();

                sb.AppendFormat(str,
                    r.Name.Replace(",", ""),
                    billingBeginDate,
                    billingEndDate,
                    numOfDays,
                    laAmt.ToString().Replace(",", ""),
                    ccAmt.ToString().Replace(",", ""),
                    pvAmt.ToString().Replace(",", ""),
                    Environment.NewLine);
                
            }
            return header + sb.ToString();
        }

        private decimal GetTotal(IEnumerable<Schedule> schedules, string paymentFrom)
        {
            var x = schedules.Where(s => s.PaymentFrom == paymentFrom).Select(k => k.AmountDue).Sum();
            return x;
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





    }
}
