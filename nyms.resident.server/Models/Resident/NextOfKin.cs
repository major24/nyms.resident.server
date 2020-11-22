using nyms.resident.server.Models.Base;
using nyms.resident.server.Models.Core;
using System.Collections;
using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class NextOfKin : IPerson
    {
        public int Id { get; set; }
        public int ResidentId { get; set; }
        public string Relationship { get; set; }
        public Address Address { get; set; }
        public IEnumerable<ContactInfo> ContactInfos { get; set; } 
    }
}