using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public int ResidentId { get; set; }
        public int NokId { get; set; }
        public string RefType { get; set; }
        public string ContactType { get; set; }
        public string Data { get; set; }
        public string Active { get; set; }
    }
}