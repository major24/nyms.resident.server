using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models.Core
{
    public class Address
    {
        public int Id { get; set; }
        public int ResidentId { get; set; }
        public int NokId { get; set; }
        public string RefType { get; set; }
        public string AddrType { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
    }
}