using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class CareHomeDivision
    {
        public int Id { get; }
        public int CareHomeId { get; set; }
        public string Name { get; set; }
    }
}