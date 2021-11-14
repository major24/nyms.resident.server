using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingActionAuditRequest
    {
        public int Id { get; set; }
        public ACTION_AUDITED_STATUS Audited { get; set; }
        public DateTime AuditedDate { get; set; }
        public int AuditedById { get; set; }
        public string Comment { get; set; }
    }
}