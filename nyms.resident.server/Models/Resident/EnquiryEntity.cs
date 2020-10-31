using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class EnquiryEntity
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public int CareHomeId { get; set; }
        public int ReferralAgencyId { get; set; }
        public int LocalAuthorityId { get; set; }
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
        public DateTime? MoveInDate { get; set; } = null;
        public DateTime? FamilyHomeVisitDate { get; set; } = null;
        public int ReservedRoomLocation { get; set; }
        public int ReservedRoomNumber { get; set; }
/*        public string Street { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string NokForeName { get; set; }
        public string NokSurName { get; set; }
        public string NokEmailAddress { get; set; }
        public string NokPhoneNumber { get; set; }
        public DateTime? ResponseDate { get; set; } = null;
        public string Response { get; set; }*/
        public string Comments { get; set; }
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}