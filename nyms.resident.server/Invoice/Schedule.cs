using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Invoice
{
    public class Schedule
    {
        public Schedule()
        {
            LocalAuthorityId = null;
        }
        public int? LocalAuthorityId { get; set; }
        public string PaymentFrom { get; set; } // ContributorName { get; set; }
        public string Description { get; set; }
        public DateTime ScheduleBeginDate { get; set; }
        public DateTime ScheduleEndDate { get; set; }
        public decimal WeeklyFee { get; set; }
        public decimal AmountDue { get; set; }
    }
}