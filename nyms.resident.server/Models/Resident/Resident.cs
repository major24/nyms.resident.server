using nyms.resident.server.Models.Core;
using System;

namespace nyms.resident.server.Models
{
    public class Resident : ResidentBase
    {
        public int Id { get; }
        public int CareHomeId { get; set; }
        public int LocalAuthorityId { get; set; }
        public string PoNumber { get; set; }
        public string LaId { get; set; }
        public Address Address { get; set; }
        // public SocialWorker SocialWorker { get; set; }
        public string SwForeName { get; set; } 
        public string SwSurName { get; set; }
        public string SwEmailAddress { get; set; }
        public string SwPhoneNumber { get; set; }
        public string CareCategory { get; set; }
        public DateTime? admissionDate { get; set; } = null;
        public DateTime? ExitDate { get; set; } = null;
        public string StayType { get; set; }
        public string CareNeeds { get; set; }
        public int RoomLocation { get; set; }
        public int RoomNumber { get; set; } // is this ID? FK
        public DateTime? FamilyHomeVisitDate { get; set; } = null;
        public string Status { get; set; }
        public DateTime? UpdatedDate { get; set; } = null;
        public DateTime? CreatedDate { get; } = null;
        public string Comments { get; set; }
    }
}