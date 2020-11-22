using NLog;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private static Logger logger = Nlogger2.GetLogger();

        public AuthenticationService(IJwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest)
        {
            // get user with user name pwd
            int expireIn = 480; // 60Min * 8 hours = 480
            logger.Info($"Authenticating , {authenticationRequest.UserName} for access.");
            var userExists = _userService.GetUserByUserNamePassword(authenticationRequest.UserName, authenticationRequest.Password).Result;  //_authenticationRepository.EnsureValidUser(model.UserName, model.Password);
            if (userExists == null)
            {
                logger.Error($"No user found for, {authenticationRequest.UserName}");
                return null;
            }
                
            try
            {
                // validate pwd with bcypt??
                bool verified = BCrypt.Net.BCrypt.Verify(authenticationRequest.Password, userExists.Password);
                if (!verified)
                {
                    logger.Error($"User could not be validated. Could be incorrect password.");
                    return null;
                }
            }
            catch(Exception ex)
            {
                logger.Error($"Faild to validate user password, {ex.Message}");
                throw;
            }

            // if not valid send error..

            // pwd is valid. get user
            var user = _userService.GetByRefereneId(userExists.ReferenceId).Result;
            var roles = user.CareHomeRoles.Select(r => r.Name);
            user.Password = string.Empty;

            return new AuthenticationResponse(user, _jwtService.GenerateJWTToken(user.ForeName, user.ReferenceId.ToString(), roles.ToArray(), expireIn)); //, GenerateJwtRefreshToken(user));
        }

        public bool ValidateToken(string token)
        {
            var simplePrinciple = _jwtService.GetPrincipal(token);
            if (simplePrinciple == null)
                return false;
            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            string username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            // You can implement more validation to check whether username exists in your DB or not or something else. 

            return true;
        }

        public Task<ClaimsIdentity> GetClaimsIdentity(string token)
        {
            string refId;
            var simplePrinciple = _jwtService.GetPrincipal(token);
            if (simplePrinciple == null)
                return null;

            var identity = simplePrinciple.Identity as ClaimsIdentity;

            var refIdClaim = identity.FindFirst(ClaimTypes.Name); // identity.FindFirst("ReferenceId");
            refId = refIdClaim?.Value;
            //to get more information from DB in order to build local identity based on username 
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, refId)
                    // you can add more claims if needed like Roles ( Admin or normal user or something else)
                };

            identity = new ClaimsIdentity(claims, "Jwt");
            return Task.FromResult(identity);
        }

        public Task<User> GetUserByRefereneId(Guid referenceId)
        {
            return _userService.GetByRefereneId(referenceId);
        }
    }
}