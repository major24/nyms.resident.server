using System;
using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class CareHomeUser
    {
        public Guid ReferenceId { get; set; }
        public string ForeName { get; set; }
        public string SurName { get; set; }
        public IEnumerable<CareHomeRole> CareHomeRoles { get; set; }
    }
}