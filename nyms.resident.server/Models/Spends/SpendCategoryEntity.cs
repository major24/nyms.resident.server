using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class SpendCategoryEntity
    {
        public int Id { get; set; }
        public int SpendMasterCategoryId { get; set; }
        public string Name { get; set; }
        public string CatCode { get; set; }
        public string Active { get; set; }
        public IEnumerable<SpendCategoryRoleEntity> SpendCategoryRoleEntities { get; set; }
    }
}