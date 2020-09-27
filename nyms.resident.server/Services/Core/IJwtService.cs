using System.Security.Claims;

namespace nyms.resident.server.Services.Core
{
    public interface IJwtService
    {
        string GenerateJWTToken(string name, string referenceId, string[] roles, int expire_in_Minutes = 30);
        string GenerateJWTToken(string name, string referenceId, int expire_in_Minutes = 30);
        ClaimsPrincipal GetPrincipal(string token);
    }
}
