﻿using nyms.resident.server.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class ResidentListResponse: ResidentBase
    {
        public int CareHomeId { get; set; }
        public string LaId { get; set; }
        public string CareNeed { get; set; }
        public string StayType { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public string Active { get; set; }
        public DateTime DischargedFromHomeDate { get; set; }
    }
}