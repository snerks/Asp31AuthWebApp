using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Asp31AuthWebApp.IntegrationTest
{
    public static class WebApplicationFactoryExtensions
    {
        private const string TestAuthenticationScheme = "Test";

        public static WebApplicationFactory<T> WithAuthentication<T>(
            this WebApplicationFactory<T> factory,
            TestClaimsProvider claimsProvider) where T : class
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(TestAuthenticationScheme)
                            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(TestAuthenticationScheme, op => { });

                    services.AddScoped<TestClaimsProvider>(_ => claimsProvider);
                });
            });
        }

        public static HttpClient CreateClientWithTestAuth<T>(
            this WebApplicationFactory<T> factory,
            TestClaimsProvider claimsProvider) where T : class
        {
            var client =
                factory.WithAuthentication(claimsProvider)
                       .CreateClient(new WebApplicationFactoryClientOptions
                       {
                           AllowAutoRedirect = false
                       });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthenticationScheme);

            return client;
        }
    }
}
