using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingRequest
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string Title { get; set; }
        public DateTime MeetingDate { get; set; }
        public int OwnerId { get; set; }

        public IEnumerable<MeetingActionRequest> MeetingActions { get; set; }
    }
}