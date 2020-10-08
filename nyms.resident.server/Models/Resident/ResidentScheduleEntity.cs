using System;

namespace nyms.resident.server.Models
{
    public class ResidentScheduleEntity
    {
        public int ResidentId { get; set; }
        public Guid ReferenceId { get; set; }
        public string ForeName { get; set; }
        public string SurName { get; set; }
        public int ScheduleId { get; set; }
        public int LocalAuthorityId { get; set; }
        public string PaymentFrom { get; set; }
        public string PaymentFromName { get; set; }
        public int PaymentTypeId { get; set; }
        public string Description { get; set; }
        public DateTime ScheduleBeginDate { get; set; }
        public DateTime ScheduleEndDate { get; set; }
        public decimal WeeklyFee { get; set; }
    }
}