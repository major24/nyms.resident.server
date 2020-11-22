using nyms.resident.server.Models.Base;
using System;

namespace nyms.resident.server.Models
{
    public class EnquiryListResponse: IPerson
    {
        public Guid ReferenceId { get; set; }
        public int CareHomeId { get; set; }
        public int ReferralAgencyId { get; set; }
        public int CareCategoryId { get; set; }
        public string CareNeed { get; set; }
        public string StayType { get; set; }
        public DateTime? MoveInDate { get; set; } = null;
        public string Status { get; set; }
    }
}