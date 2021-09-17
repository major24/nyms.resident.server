using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingActionItemDto
    {
        public int Id { get; set; }
        public int MeetingCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsAdhoc { get; set; }
    }
}