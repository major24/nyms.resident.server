using nyms.resident.server.Models.Core;
using nyms.resident.server.Models.Resident;
using System;

namespace nyms.resident.server.Models
{
    public class Enquiry : ResidentBase
    {
        public int CareHomeId { get; set; }
        public Address Address { get; set; }
        public SocialWorker SocialWorker { get; set; }
        public string CareCategory { get; set; }
        public DateTime? EnquiryDate { get; set; } = null;
        public DateTime? MoveInDate { get; set; } = null;
        public string StayType { get; set; }
        public string CareNeeds { get; set; }
        public int ReservedRoomLocation { get; set; }
        public int ReservedRoomNumber { get; set; } // is this ID? FK
        public DateTime? FamilyHomeVisitDate { get; set; } = null;
        public DateTime? ResponseDate { get; set; } = null;
        public string Response { get; set; }
        public string Status { get; set; }
        public DateTime? UpdatedDate { get; set; } = null;
        public DateTime? CreatedDate { get; } = null;
        public string Comments { get; set; }
    }
}