using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime MeetingDate { get; set; }
        public int OwnerId { get; set; }
        public string Status { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public IEnumerable<MeetingAction> MeetingActions { get; set; }

        // public string Active { get; set; }
        // public int MeetingCategoryId { get; set; }
        // public IEnumerable<int> MeetingAgendaIds { get; set; }
        // public IEnumerable<MeetingAgenda> MeetingAgendas { get; set; }
    }
}