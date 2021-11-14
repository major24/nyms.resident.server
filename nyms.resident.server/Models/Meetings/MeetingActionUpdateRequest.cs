using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingActionUpdateRequest
    {
        public int Id { get; set; } // MeetingAction tbl id
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Priority { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}