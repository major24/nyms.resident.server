using nyms.resident.server.Models.Core;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class ResidentRequest : ResidentBase
    {
        public int CareHomeId { get; set; }
        public int LocalAuthorityId { get; set; }
        public string NhsNumber { get; set; }
        public string PoNumber { get; set; }
        public string LaId { get; set; }
        public string NymsId { get; set; }
        public int CareCategoryId { get; set; }
        public string CareCategory { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? ExitDate { get; set; } = null;
        public string StayType { get; set; }
        public string CareNeed { get; set; }
        public int RoomLocation { get; set; }
        public int RoomNumber { get; set; }
        public DateTime? FamilyHomeVisitDate { get; set; } = null;
        public string Status { get; set; }
        public string Comments { get; set; }
        public SocialWorker SocialWorker { get; set; }
        public Address Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<NextOfKin> NextOfKins { get; set; }
        public Guid EnquiryReferenceId { get; set; }
        public int UpdatedBy { get; set; }
        public int CareHomeDivisionId { get; set; }
    }
}