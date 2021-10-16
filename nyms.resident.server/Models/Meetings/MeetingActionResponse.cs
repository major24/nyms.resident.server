using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingActionResponse: MeetingAction
    {
        public string OwnerForename { get; set; }
        // public string OwnerSurname { get; set; }
        public string CompletedByForename { get; set; }
        // public string CompletedBySurname { get; set; }
/*        public string UpdatedByForename { get; set; }
        public string UpdatedBySurname { get; set; }*/
        public string InspectedByForename { get; set; }
        // public string InspectedBySurname { get; set; }
        public string NewComment { get; set; }
        public IEnumerable<MeetingActionComment> Comments { get; set; }
       /* public string NewCommentType { get; set; }*/
    }
}