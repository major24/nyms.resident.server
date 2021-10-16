using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingActionInspectRequest
    {
        public int Id { get; set; }
        public string Inspected { get; set; }
        public DateTime InspectedDate { get; set; }
        public MeetingActionComment MeetingActionComment { get; set; }
        public int InspectedById { get; set; }
    }
}