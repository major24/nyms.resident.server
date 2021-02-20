using nyms.resident.server.Models.Core;
using System;
using System.Collections.Generic;

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
        public int CareCategoryId { get; set; }
        public string CareNeed { get; set; }
        public string StayType { get; set; }
        public int RoomLocation { get; set; }
        public int RoomNumber { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? ExitDate { get; set; } = null;
        public string ExitReason { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public int UpdatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid EnquiryReferenceId { get; set; }

        public Address Address { get; set; }
        public IEnumerable<NextOfKin> NextOfKins { get; set; }
        public SocialWorker SocialWorker { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}