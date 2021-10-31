using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class MeetingCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<MeetingActionItemResponse> MeetingActionItems { get; set; }
    }
}