using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingActionResponse
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public int MeetingCategoryId { get; set; }
        public int MeetingActionItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsAdhoc { get; set; }
        public int OwnerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Priority { get; set; }
        public string Forename { get; set; }
        public string Completed { get; set; }
    }
}