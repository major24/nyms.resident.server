using nyms.resident.server.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Invoice
{
    public class InvoiceValidationsReportResponse
    {
        public int BillingCycleId { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public int? LocalAuthorityId { get; set; }
        public string LocalAuthority { get; set; }
        public int PaymentTypeId { get; set; }
        public string Description { get; set; }
        public int ResidentId { get; set; }
        public string Name { get; set; } 
        public decimal AmountDue { get; set; }
        public string Validated { get; set; }
        public decimal ValidatedAmount { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}