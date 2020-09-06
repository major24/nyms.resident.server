using nyms.resident.server.Models.Base;
using System;

namespace nyms.resident.server.Models
{
    public abstract class ResidentBase : IPerson
    {
        public string MiddleName { get; set; }
        public DateTime? Dob { get; set; } = null;
        public string Gender { get; set; }
        public string MartialStatus { get; set; }
    }
}