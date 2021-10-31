using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingActionPendingJobsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Priority { get; set; }
        public string Forename { get; set; }
        public string CategoryName { get; set; }
        public string Title { get; set; }
    }
}