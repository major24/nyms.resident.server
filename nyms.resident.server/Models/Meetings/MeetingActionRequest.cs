using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingActionRequest
    {
        // meeting action item
        public int Id { get; set; } // MeetingAction tbl id
        public int MeetingCategoryId { get; set; }
        public int MeetingActionItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // public string IsAdhoc { get; set; }
        // meeting action
        public int OwnerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Priority { get; set; }

        // frequency and reoccurrence and fortnight / weekly settings
        public ACTION_FREQUENCY Frequency { get; set; }
        public int Repetitive { get; set; }
        // Supporting property to manage data at client side
        public bool Checked { get; set; }
    }
}