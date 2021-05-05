using nyms.resident.server.Models;
using System.Collections.Generic;

namespace nyms.resident.server.Services.Interfaces
{
    public interface ISpendCategoriesService
    {
        IEnumerable<SpendMasterCategory> GetSpendMasterCategories();
        IEnumerable<SpendCategoryEntity> GetSpendCategoryEntities();
        IEnumerable<SpendCategory> GetSpendCategories();
        SpendCategory Insert(SpendCategoryRequest spendCategoryRequest);
        SpendCategory Update(SpendCategoryRequest spendCategoryRequest);
    }
}
