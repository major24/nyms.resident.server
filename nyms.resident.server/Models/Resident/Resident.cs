using nyms.resident.server.Models.Core;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class Resident : ResidentBase
    {
        public int Id { get; }
        public int CareHomeId { get; set; }
        public Address Address { get; set; }
        public SocialWorker SocialWorker { get; set; }
        public string CareCategory { get; set; }
        public DateTime? AdminitionDate { get; set; } = null;
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