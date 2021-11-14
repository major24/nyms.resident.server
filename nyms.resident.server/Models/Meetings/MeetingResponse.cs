using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingResponse
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string Title { get; set; }
        public DateTime MeetingDate { get; set; }
        public int OwnerId { get; set; }
        public IEnumerable<MeetingActionResponse> MeetingActions { get; set; }
    }
}