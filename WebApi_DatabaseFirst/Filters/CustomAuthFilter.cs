using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace WebApi_DatabaseFirst.Filters
{
    public class CustomAuthFilter : Attribute, IAuthenticationFilter
    { 
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            System.Net.Http.HttpRequestMessage request = context.Request;
            System.Net.Http.Headers.AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if(authorization == null || authorization.Scheme != "Bearer")
            {
                return;
            }

            string token = authorization.Parameter;
            IPrincipal principal = await AuthenticateJwtToken(token);
            if (principal == null) return;
            else
                context.Principal = principal;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellation)
        {
            return Task.FromResult(0);
        }

        private static bool ValidateToken(string token, out string username)
        {
            ClaimsPrincipal simplePrincipal = TokenManager.GetPrincipal(token);
            ClaimsIdentity identity = simplePrincipal?.Identity as ClaimsIdentity;

            if (identity == null)
            {
                username = null;
                return false;
            }

            if (!identity.IsAuthenticated)
            {
                username = null;
                return false;
            }

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            return true;
        }


        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            if(ValidateToken (token, out string username))
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

       
    }

}