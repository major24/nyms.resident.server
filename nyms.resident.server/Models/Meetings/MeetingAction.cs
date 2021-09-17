using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingAction : MeetingActionItem
    {
        // public int MeetingId { get; set; }
        public int MeetingActionItemId { get; set; }
        public int OwnerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Priority { get; set; }
        public string Completed { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public string Status { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int InspectedById { get; set; }
        public DateTime? InspectedDate { get; set; }

        // Supporting property to manage data at client side
        // public bool Checked { get; set; }
    }
}