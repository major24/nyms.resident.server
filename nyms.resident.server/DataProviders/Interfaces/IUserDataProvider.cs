using nyms.resident.server.Models;
using System;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IUserDataProvider
    {
        Task<User> GetById(int id);
        Task<User> GetUserByReferenceId(Guid referenceId);
        Task<User> GetUserByUserNamePassword(string userName, string password);
        Task<CareHomeUser> GetCareHomeUser(Guid referenceId);
        void SetPassword(Guid referenceId, string password);
    }
}