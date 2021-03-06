﻿using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
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
    [AdminAuthenticationFilter]
    public class InvoicesController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
        }


        // api/invoices/...
        // Main admin report with checkable functionality. shows validated amounts and comments
        [HttpGet]
        [Route("api/invoices/localAuthorities/{localAuthorityId}/billingCycles/{billingCycleId}")]
        public IHttpActionResult GetInvoicesByBillingCycle(int localAuthorityId, int billingCycleId)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Invoice by billing cycle requested by {user?.ForeName}");

            if (localAuthorityId <= 0) throw new ArgumentNullException(nameof(localAuthorityId));
            if (billingCycleId <= 0) throw new ArgumentNullException(nameof(billingCycleId));

            var invData = this._invoiceService.GetInvoiceData(billingCycleId);

            if (invData == null)
            {
                return NotFound();
            }
            return Ok(invData);
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
                DateTime.TryParse(billingBeginDate, out DateTime billingBegin);
                DateTime.TryParse(billingEndDate, out DateTime billingEnd);

                if (billingBegin == null || billingEnd == null) throw new ArgumentException("Invalid dates");

                var invData = this._invoiceService.GetInvoiceData(billingBegin, billingEnd);

                var detailCsvReport = CreateDetailCsvReport(invData.InvoiceResidents, billingBeginDate, billingEndDate);
                var singleLineCsvReport = CreateSingleLineCsvReport(invData.InvoiceResidents, billingBeginDate, billingEndDate);

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

        [HttpGet]
        [Route("api/invoices/billing-cycles")]
        public IHttpActionResult GetBillingCycles()
        {
            var billingCycles = _invoiceService.GetBillingCycles().Result;

            if (billingCycles == null)
            {
                return NotFound();
            }
            return Ok(billingCycles);
        }

        ///api/invoices/validations
        [HttpPost]
        [Route("api/invoices/validations")]
        public IHttpActionResult UpdateValidatedInvoices([FromBody] InvoiceValidatedEntity[] invoiceValidatedEntities)
        {
            var user = System.Threading.Thread.CurrentPrincipal as SecurityPrincipal;
            logger.Info($"Invoice is validated by {user?.ForeName}");

            if (invoiceValidatedEntities == null || !invoiceValidatedEntities.Any()) throw new ArgumentNullException(nameof(invoiceValidatedEntities));

            this._invoiceService.UpdateInvoicesValidated(invoiceValidatedEntities);

            return Ok(invoiceValidatedEntities);
        }

        [HttpPost]
        [Route("api/invoices/comments")]
        public IHttpActionResult InsertValidationComments([FromBody] InvoiceCommentsEntity invoiceCommentsEntity)
        {
            this._invoiceService.InsertInvoiceComments(invoiceCommentsEntity);

            return Ok(invoiceCommentsEntity);
        }
        



        private string CreateDetailCsvReport(IEnumerable<InvoiceResident> invResidents, string billingBeginDate, string billingEndDate)
        {
            string header = "Name,Payment Provider Id,Local Authority,Description,Start Date,End Date,Num.Of Days,Weekly Fee,Amount Due," + Environment.NewLine;

            StringBuilder sb = new StringBuilder();
            string str = "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}";
            foreach (var r in invResidents)
            {
                foreach (var s in r.SchedulePayments)
                {
                    sb.AppendFormat(str,
                        r.Name.Replace(",", ""),
                        s.PaymentProviderId,
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
                var laAmt = GetTotal(r.SchedulePayments, 1); // LA
                var ccAmt = GetTotal(r.SchedulePayments, 2); // CC
                var pvAmt = GetTotal(r.SchedulePayments, 3); // PV
                var numOfDays = r.SchedulePayments.Select(s => s.NumberOfDays).FirstOrDefault();

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

        private decimal GetTotal(IEnumerable<SchedulePayment> schedules, int PaymentProviderId)
        {
            var x = schedules.Where(s => s.PaymentProviderId == PaymentProviderId).Select(k => k.AmountDue).Sum();
            return x;
        }


    }
}
