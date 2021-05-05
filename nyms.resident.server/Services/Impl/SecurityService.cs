using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Services.Impl
{
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityDataProvider _securityDataProvider;
        public SecurityService(ISecurityDataProvider securityDataProvider)
        {
            _securityDataProvider = securityDataProvider ?? throw new ArgumentNullException(nameof(securityDataProvider));
        }

        public IEnumerable<Role> GetRoles()
        {
            return _securityDataProvider.GetRoles();
        }

        public IEnumerable<UserRolePermission> GetRolePermissions(int userId)
        {
            return _securityDataProvider.GetRolePermissions(userId);
        }
    }
}