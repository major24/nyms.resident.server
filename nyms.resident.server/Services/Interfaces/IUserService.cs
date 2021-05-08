using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        Task<User> GetById(int id);
        Task<User> GetByRefereneId(Guid referenceId);
        Task<User> GetUserByUserNamePassword(string userName, string password);
        void CreateUser(User user);
        void SetPassword(Guid referenceId, string password);
        void AssignRoles(int userId, IEnumerable<UserRolePermission> userRolePermissions);
    }
}
