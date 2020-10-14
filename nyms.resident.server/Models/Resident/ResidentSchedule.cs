using nyms.resident.server.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class ResidentSchedule : IPerson
    {
        public int ResidentId { get; set; }
        public int LocalAuthorityId { get; set; }
        public string PaymentFromName { get; set; }
        public IEnumerable<SchedulePayment> Schedules { get; set; }
    }
}