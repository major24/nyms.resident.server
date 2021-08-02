using System;

namespace nyms.resident.server.Models
{
    public class SpendComments
    {
        public int Id { get; set; }
        public int SpendId { get; set; }
        public string Comments { get; set; }
        public Status Status { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByName { get; set; }
    }
}