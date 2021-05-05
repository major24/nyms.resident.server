using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class SpendCategory
    {
        public int Id { get; set; }
        public int SpendMasterCategoryId { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public IEnumerable<Role> Roles { get; set; }

        /* public string Period { get; set; }
         public string PoPrefix { get; set; }*/
        /*        public int SpendCategoryCareHomeId { get; set; }
                public int CareHomeId { get; set; }
                public string MasterCategoryName { get; set; }
                public string CareHomeName { get; set; }*/
    }
}