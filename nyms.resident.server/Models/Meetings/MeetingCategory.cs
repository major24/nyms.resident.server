using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Active { get; set; }
        public IEnumerable<MeetingActionItem> MeetingActionItems { get; set; }
    }
}