using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class ResidentDischargeExitRequest
    {
        public Guid ReferenceId { get; set; }
        public DateTime? ExitDate { get; set; } = null;
        public DateTime DischargedFromHomeDate { get; set; }
    }
}