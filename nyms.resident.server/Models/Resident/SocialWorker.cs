using nyms.resident.server.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class SocialWorker : IPerson
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}