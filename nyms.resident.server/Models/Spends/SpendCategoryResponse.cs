using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models.Spends
{
    public class SpendCategoryResponse
    {
        public int Id { get; set; }
        public int SpendMasterCategoryId { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}