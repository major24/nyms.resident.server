﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class CareHome
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string Name { get; set; }
        public string ChCode { get; set; }
        public IEnumerable<CareHomeDivision> CareHomeDivisions { get; set; }
    }
}