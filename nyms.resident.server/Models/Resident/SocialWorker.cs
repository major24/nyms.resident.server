using nyms.resident.server.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class SocialWorker : IPerson
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}