using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Invoice
{
    public class ValidatedInvoiceEntity
    {

        public int Id { get; set; }
        public int LocalAuthorityId { get; set; }
        public int BillingCycleId { get; set; }
        public int ResidentId { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal AmountDue { get; set; }
        public string Validated { get; set; }
        public decimal TransactionAmount{ get; set; }
        public int UpdatedById { get; set; }
        public string Comments { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}