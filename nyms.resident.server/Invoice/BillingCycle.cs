using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Invoice
{
    public class BillingCycle
    {
        public int Id { get; set; }
        public int LocalAuthorityId { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime BillDate { get; set; }
        // public string Active { get; set; }
    }
}