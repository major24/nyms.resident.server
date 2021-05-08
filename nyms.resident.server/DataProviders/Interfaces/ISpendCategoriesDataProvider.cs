using nyms.resident.server.Models;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface ISpendCategoriesDataProvider
    {
        IEnumerable<SpendMasterCategory> GetSpendMasterCategories();
        IEnumerable<SpendCategoryEntity> GetSpendCategoryEntities();
        IEnumerable<SpendCategory> GetSpendCategoryAndRoles();
        SpendCategoryEntity Insert(SpendCategoryEntity spendCategoryEntity);
        SpendCategoryEntity Update(SpendCategoryEntity spendCategoryEntity);
        IEnumerable<SpendCategoryRoleEntity> GetSpendCategoryRoleEntities();
    }
}
