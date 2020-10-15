using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class ScheduleEntity
    {
        public int Id { get; set; }
        public int ResidentId { get; set; }
        public int? LocalAuthorityId { get; set; }
        public int PaymentProviderId { get; set; }
        public int PaymentTypeId { get; set; }
        public string Description { get; set; }
        public DateTime ScheduleBeginDate { get; set; }
        public DateTime ScheduleEndDate { get; set; }
        public decimal WeeklyFee { get; set; }
    }
}