using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class ResidentEntity
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public int CareHomeId { get; set; }
        public int LocalAuthorityId { get; set; }
        public string NhsNumber { get; set; }
        public string PoNumber { get; set; }
        public string LaId { get; set; }
        public string NymsId { get; set; }
        public string ForeName { get; set; }
        public string SurName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? Dob { get; set; } = null;
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string SwForeName { get; set; }
        public string SwSurName { get; set; }
        public string SwEmailAddress { get; set; }
        public string SwPhoneNumber { get; set; }
        public int CareCategoryId { get; set; }
        public string CareNeed { get; set; }
        public string StayType { get; set; }
        public int RoomLocation { get; set; }
        public int RoomNumber { get; set; }
        public DateTime? AdmissionDate { get; set; } = null;
        public DateTime? ExitDate { get; set; } = null;
        public string ExitReason { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public int UpdatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}