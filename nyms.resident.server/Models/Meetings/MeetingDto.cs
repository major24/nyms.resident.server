using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingDto
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime MeetingDate { get; set; }
        public int OwnerId { get; set; }

        public IEnumerable<MeetingActionDto> MeetingActions { get; set; }
        // When an action is deleted on [Update] scenario, this will pass those deleted [action] ids, to mark as deleted.
        public int[] DeletedIds { get; set; }
    }
}