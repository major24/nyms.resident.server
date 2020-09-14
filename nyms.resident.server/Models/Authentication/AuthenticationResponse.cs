using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class AuthenticationResponse
    {
        public string ReferenceId { get; set; }
        public string ForeName { get; set; }
        public string SurName { get; set; }
        public string JwtToken { get; set; }
        public IEnumerable<CareHomeRole> Roles { get; set; }
        // public string RefreshToken { get; set; }

        public AuthenticationResponse(User user, string jwtToken) //, string refreshToken)
        {
            ReferenceId = user.ReferenceId.ToString();
            ForeName = user.ForeName;
            SurName = user.SurName;
            Roles = user.CareHomeRoles;
            JwtToken = jwtToken;
            // RefreshToken = refreshToken;
        }
    }
}