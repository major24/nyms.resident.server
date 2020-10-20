using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class EnquiryAction
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public string Action { get; set; }
        public DateTime? ActionDate { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }
        public string UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}