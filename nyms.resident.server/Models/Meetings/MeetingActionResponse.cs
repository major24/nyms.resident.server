using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingActionResponse // : MeetingAction
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public int MeetingCategoryId { get; set; }
        public int MeetingActionItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsAdhoc { get; set; }
        public int OwnerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Priority { get; set; }
        public string Forename { get; set; }


/*        public string Completed { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Inspected { get; set; }
        public int InspectedById { get; set; }
        public DateTime? InspectedDate { get; set; }
        public string InspectionStatus { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }*/


        // public string CompletedByForename { get; set; }
        // public string CompletedBySurname { get; set; }
/*        public string UpdatedByForename { get; set; }
        public string UpdatedBySurname { get; set; }*/
        // public string InspectedByForename { get; set; }
        // public string InspectedBySurname { get; set; }
/*        public string NewComment { get; set; }
        public IEnumerable<MeetingActionComment> Comments { get; set; }*/
       /* public string NewCommentType { get; set; }*/
    }
}