using nyms.resident.server.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IAuthenticationService
    {
        AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest);
        bool ValidateToken(string token);
        Task<ClaimsIdentity> GetClaimsIdentity(string token);
        Task<User> GetUserByRefereneId(Guid referenceId);
    }
}
