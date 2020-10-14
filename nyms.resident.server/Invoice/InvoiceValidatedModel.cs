using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Invoice
{
    public class InvoiceValidatedModel
    {
        public int Id { get; set; }
        public int BillingCycleId { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal AmountDue { get; set; }
        public string Validated { get; set; }
        public decimal ValidatedAmount { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}