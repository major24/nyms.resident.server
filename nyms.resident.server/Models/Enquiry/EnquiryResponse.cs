using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class EnquiryResponse: ResidentBase
    {
        public int CareHomeId { get; set; }
        public int ReferralAgencyId { get; set; }
        public int LocalAuthorityId { get; set; }
        public int CareCategoryId { get; set; }
        public SocialWorker SocialWorker { get; set; }
        public string CareNeed { get; set; }
        public string StayType { get; set; }
        public DateTime? MoveInDate { get; set; } = null;
        public DateTime? FamilyHomeVisitDate { get; set; } = null;
        public int RoomLocation { get; set; }
        public int RoomNumber { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public string CareCategoryName { get; set; }
        public IEnumerable<EnquiryAction> EnquiryActions { get; set; }
    }
}