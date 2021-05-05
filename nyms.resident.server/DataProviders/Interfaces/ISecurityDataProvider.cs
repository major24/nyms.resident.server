using nyms.resident.server.Models;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface ISecurityDataProvider
    {
        IEnumerable<Role> GetRoles();
        IEnumerable<UserRolePermission> GetRolePermissions(int userId);
    }
}
