﻿using System;

namespace nyms.resident.server.Models
{
    public class CareHomeRole: Role
    {
        // public string RoleName { get; set; }
        public string CareHomeName { get; set; }
        public Guid CareHomeReferenceId { get; set; }
    }
}