﻿using NLog;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Models.Core;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace nyms.resident.server.Filters
{
    public class AdminAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        private static Logger logger = Nlogger2.GetLogger();
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public AdminAuthenticationFilter() { }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // checking request header value having required scheme "Bearer" or not.
            if (authorization == null || authorization.Scheme != "Bearer" || string.IsNullOrEmpty(authorization.Parameter))
            {
                logger.Error(Constants.TOKEN_MISSING);
                context.ErrorResult = new AuthFailureResult(Constants.TOKEN_MISSING, request);
            }

            // Getting Token value from header values.
            var token = authorization.Parameter;

            var _authenticationService = context.Request.GetDependencyScope().GetService(typeof(IAuthenticationService)) as IAuthenticationService;
            var _userService = context.Request.GetDependencyScope().GetService(typeof(IUserService)) as IUserService;

            if (!_authenticationService.ValidateToken(token))
            {
                logger.Error(Constants.TOKEN_INVALID);
                context.ErrorResult = new AuthFailureResult(Constants.TOKEN_INVALID, request);
            }

            var identity = _authenticationService.GetClaimsIdentity(token);

            if (identity == null)
            {
                logger.Error(Constants.TOKEN_INVALID_IDTY);
                context.ErrorResult = new AuthFailureResult(Constants.TOKEN_INVALID_IDTY, request);
            }
            else
            {
                logger.Info($"{Constants.REQUST_ACCESS_FOR} {identity.Result.Name}");
                var refid = identity.Result.Name;
                var user = _userService.GetByRefereneId(new Guid(refid)).Result;

                var role = user.CareHomeRoles.Where(r => r.Name == "Admin").FirstOrDefault();
                if (role == null)
                {
                    logger.Error($"{Constants.TOKEN_ACCESS_DENIED_NO_ROLE_FOR} {identity.Result.Name}");
                    context.ErrorResult = new AuthFailureResult($"{Constants.TOKEN_ACCESS_DENIED_NO_ROLE_FOR} {identity.Result.Name}", request);
                }

                IPrincipal identityUser = new ClaimsPrincipal(identity.Result);
                logger.Info($"{Constants.REQUST_ACCESS_GRANTED_FOR} {identity.Result.Name}");

                IPrincipal securityPrincipal = new SecurityPrincipal() {
                    Id = user.Id, 
                    ReferenceId = user.ReferenceId, 
                    ForeName = user.ForeName, 
                    SurName = user.SurName, 
                    Roles = user.CareHomeRoles
                };

                // context.Principal = identityUser;
                context.Principal = securityPrincipal;
                HttpContext.Current.User = securityPrincipal;
                System.Threading.Thread.CurrentPrincipal = securityPrincipal;
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
