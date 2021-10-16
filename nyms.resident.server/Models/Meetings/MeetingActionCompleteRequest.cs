using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Model
{
    public class MeetingActionCompleteRequest
    {
        public int Id { get; set; }
        public string Completed { get; set; }
        public DateTime CompletedDate { get; set; }
        public MeetingActionComment MeetingActionComment { get; set; }
        public int CompletedById { get; set; }
    }
}