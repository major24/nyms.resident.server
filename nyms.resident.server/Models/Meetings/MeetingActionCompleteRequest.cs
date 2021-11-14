using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingActionCompleteRequest
    {
        public int Id { get; set; }
        public ACTION_COMPLETED_STATUS Completed { get; set; }
        public DateTime CompletedDate { get; set; }
        public int CompletedById { get; set; }
        public string Comment { get; set; }
    }
}