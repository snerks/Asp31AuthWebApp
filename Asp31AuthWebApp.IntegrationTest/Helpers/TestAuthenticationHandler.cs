using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Asp31AuthWebApp.IntegrationTest
{
    public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IList<Claim> _claims;

        public TestAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            TestClaimsProvider claimsProvider) : base(options, logger, encoder, clock)
        {
            _claims = claimsProvider.Claims;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            const string TestAuthenticationType = "Test";
            const string TestAuthenticationScheme = "Test";

            var claimsIdentity = new ClaimsIdentity(_claims, TestAuthenticationType);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authenticationTicket = new AuthenticationTicket(claimsPrincipal, TestAuthenticationScheme);

            var authenticateResult = AuthenticateResult.Success(authenticationTicket);

            return Task.FromResult(authenticateResult);
        }
    }
}
