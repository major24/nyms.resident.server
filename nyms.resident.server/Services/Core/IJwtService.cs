using System.Security.Claims;

namespace nyms.resident.server.Services.Core
{
    public interface IJwtService
    {
        string GenerateJWTToken(string name, string referenceId, int expire_in_Minutes = 30);
        ClaimsPrincipal GetPrincipal(string token);
    }
}
