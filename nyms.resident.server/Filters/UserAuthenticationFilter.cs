using nyms.resident.server.Models.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace nyms.resident.server.Filters
{
    public class UserAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public UserAuthenticationFilter()
        {
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // checking request header value having required scheme "Bearer" or not.
            if (authorization == null || authorization.Scheme != "Bearer" || string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthFailureResult("JWT Token is Missing", request);
                return;
            }

            // Getting Token value from header values.
            var token = authorization.Parameter;

            var _authenticationService = context.Request.GetDependencyScope().GetService(typeof(IAuthenticationService)) as IAuthenticationService;
            var _userService = context.Request.GetDependencyScope().GetService(typeof(IUserService)) as IUserService;

            if (!_authenticationService.ValidateToken(token))
            {
                context.ErrorResult = new AuthFailureResult("Invalid JWT Token", request);
            }

            var identity = _authenticationService.GetClaimsIdentity(token);

            if (identity == null)
            {
                context.ErrorResult = new AuthFailureResult("Invalid Identity in JWT Token", request);
            }
            else
            {
                var refid = identity.Result.Name;
                var user = _userService.GetCareHomeUser(new Guid(refid)).Result;

                var role = user.CareHomeRoles.Where(r => r.RoleName == "Admin" || r.RoleName == "Manager").FirstOrDefault();
                if (role == null)
                {
                    context.ErrorResult = new AuthFailureResult($"Role Missing: Access denied for {user.ForeName}", request);
                }
                
                IPrincipal identityUser = new ClaimsPrincipal(identity.Result);
                context.Principal = identityUser;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }
        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            // context.ChallengeWith("Bearer", parameter);
            context.Request.Headers.Add("Bearer", parameter);
        }
    }
}




