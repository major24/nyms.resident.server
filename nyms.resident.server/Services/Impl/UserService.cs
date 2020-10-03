using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Filters;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Impl
{
    [UserAuthenticationFilter]
    public class UserService : IUserService
    {
        private readonly IUserDataProvider _userDataProvider;
        public UserService(IUserDataProvider userDataProvider) 
        {
            _userDataProvider = userDataProvider ?? throw new ArgumentNullException(nameof(userDataProvider));
        }

        public  Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByRefereneId(Guid referenceId)
        {
            return _userDataProvider.GetUserByReferenceId(referenceId);
        }

        public Task<User> GetUserByUserNamePassword(string userName, string password)
        {
            return _userDataProvider.GetUserByUserNamePassword(userName, password);
        }

        public void SetPassword(Guid referenceId, string password)
        {
            _userDataProvider.SetPassword(referenceId, BCrypt.Net.BCrypt.HashPassword(password));
        }

        public IEnumerable<User> GetUsers()
        {
            try
            {
                return this._userDataProvider.GetUsers();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}