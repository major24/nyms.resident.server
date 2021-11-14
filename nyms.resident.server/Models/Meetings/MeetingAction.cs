using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingAction
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public int MeetingCategoryId { get; set; }
        public int MeetingActionItemId { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Priority { get; set; }
        public string Completed { get; set; }
        public int CompletedById { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Audited { get; set; }
        public int AuditedById { get; set; }
        public DateTime? AuditedDate { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ActionStatus { get; set; }

        public string Name { get; set; }
        public string IsAdhoc { get; set; }
        public int CreatedById { get; set; }
    }
}