﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models.Base
{
    public abstract class IPerson
    {
        public string ForeName { get; set; }
        public string SurName { get; set; }
    }
}