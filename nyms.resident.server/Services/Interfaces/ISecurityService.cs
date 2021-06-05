using nyms.resident.server.Models;
using System.Collections.Generic;

namespace nyms.resident.server.Services.Interfaces
{
    public interface ISecurityService
    {
        IEnumerable<Role> GetRoles();
        IEnumerable<UserRolePermission> GetRolePermissions(int userId);

        IEnumerable<int> GetSpendCategoryRoleIds(int userId);
    }
}
