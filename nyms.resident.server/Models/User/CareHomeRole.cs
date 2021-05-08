using System;

namespace nyms.resident.server.Models
{
    public class CareHomeRole: Role
    {
        public string CareHomeName { get; set; }
        public Guid CareHomeReferenceId { get; set; }
    }
}