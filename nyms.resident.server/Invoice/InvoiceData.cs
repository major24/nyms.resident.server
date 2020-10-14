using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Invoice
{
    public class InvoiceData
    {
        public int Id { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BillingCycleId { get; set; }
        public DateTime BillingDate { get; set; }
        public int NumberOfDays { get; set; }
        public IEnumerable<InvoiceResident> InvoiceResidents { get; set; }
    }
}