using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace nyms.resident.server.Services.Core
{
    public class JwtService: IJwtService
    {
        public const string SecretKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ97eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE1OTYzMzU3MzgsImV";
        public string GenerateJWTToken(string name, string referenceId, int expire_in_Minutes = 30)
        {
            var symmetric_Key = Convert.FromBase64String(SecretKey);
            var token_Handler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var securitytokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, referenceId),
                            new Claim("ForeName", name)
                        }),

                Expires = now.AddMinutes(Convert.ToInt32(expire_in_Minutes)),
                Issuer = ConfigurationManager.AppSettings["jwtIssuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetric_Key), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = token_Handler.CreateToken(securitytokenDescriptor);
            var token = token_Handler.WriteToken(stoken);

            return token;
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(SecretKey);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = ConfigurationManager.AppSettings["jwtIssuer"],
                    //ClockSkew = ze
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = jwtTokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}