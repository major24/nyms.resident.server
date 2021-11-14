using nyms.resident.server.Models;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface ISecurityDataProvider
    {
        IEnumerable<UserRolePermission> GetRolePermissions(int userId);
        IEnumerable<int> GetSpendCategoryRoleIds(int userId);
        IEnumerable<UserRoleAccess> GetUserRoleAccesses();
    }
}
