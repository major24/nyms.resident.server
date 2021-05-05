using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IUserDataProvider
    {
        Task<User> GetById(int id);
        Task<User> GetUserByReferenceId(Guid referenceId);
        Task<User> GetUserByUserNamePassword(string userName, string password);
        void CreateUser(string userName, string password, User user);
        void SetPassword(Guid referenceId, string password);
        IEnumerable<User> GetUsers();
        void AssignRoles(int userId, IEnumerable<UserRolePermission> userRolePermissions);
    }
}