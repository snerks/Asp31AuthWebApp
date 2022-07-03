using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Asp31AuthWebApp.IntegrationTest
{
    public class WeatherForecastControllerTests // : TestBase
    {
        //private readonly HttpClient _client;
        //private readonly TestApplicationFactory<Startup, FakeStartup> _factory;

        //public WeatherForecastControllerTests(TestApplicationFactory</* Startup, */ FakeStartup> factory) : base(factory)
        //{
        //    //Factory = factory;
        //    //_factory = factory;
        //    //_client = factory.CreateClient(new WebApplicationFactoryClientOptions
        //    //{
        //    //    AllowAutoRedirect = false
        //    //});
        //}

        //public TestApplicationFactory<Startup, FakeStartup> Factory
        //{
        //    get;
        //}

        [Theory]
        [InlineData("weatherforecast")]
        public async Task Get_Anonymous_CannotAccessUrl(string url)
        {
            // Arrange
            // Default client option values are shown
            var clientOptions = new WebApplicationFactoryClientOptions();
            //clientOptions.AllowAutoRedirect = true;

            ////clientOptions.BaseAddress = new Uri("http://localhost");
            //clientOptions.BaseAddress = new Uri("http://localhost:43218/");
            //// http://localhost:5000/weatherforecast

            //clientOptions.HandleCookies = true;
            //clientOptions.MaxAutomaticRedirections = 7;

            //var factory = new TestApplicationFactory<Asp31AuthWebApp.Startup>();
            var factory = new TestApplicationFactory<Asp31AuthWebApp.Startup>();

            using var client = factory.CreateClient(clientOptions);

            // Act
            using var response = await client.GetAsync(url);

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("weatherforecast")]
        public async Task Get_Authenticated_CanAccessUrl(string url)
        {
            // Arrange
            var factory = new TestApplicationFactory<Asp31AuthWebApp.Startup>();
            var claimsProvider = TestClaimsProvider.WithAdminClaims();

            //using var client = factory.CreateClient();
            using var client = factory.CreateClientWithTestAuth(claimsProvider);

            // Act
            using var response = await client.GetAsync(url);

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            var items = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(responseString);
            items.Any().Should().BeTrue();
        }

        //[Fact]
        //public async Task BasicIntegrationTest()
        //{
        //    // Arrange
        //    var hostBuilder = new HostBuilder()
        //        .ConfigureWebHost(webHost =>
        //        {
        //            // Add TestServer
        //            webHost.UseTestServer();

        //            // Specify the environment
        //            webHost.UseEnvironment("Test");

        //            webHost.Configure(app => app.Run(async ctx => await ctx.Response.WriteAsync("Hello World!")));
        //        });

        //    // Create and start up the host
        //    var host = await hostBuilder.StartAsync();

        //    // Create an HttpClient which is setup for the test host
        //    var client = host.GetTestClient();

        //    // Act
        //    var response = await client.GetAsync("/");

        //    // Assert
        //    var responseString = await response.Content.ReadAsStringAsync();
        //    responseString.Should().Be("Hello World!");
        //}

        //[Fact]
        //public async Task BasicEndPointTest()
        //{
        //    // Arrange
        //    var hostBuilder = new HostBuilder()
        //        .ConfigureWebHost(webHost =>
        //        {
        //            // Add TestServer
        //            webHost.UseTestServer();
        //            webHost.UseStartup<Asp31AuthWebApp.Startup>();
        //        });

        //    // Create and start up the host
        //    var host = await hostBuilder.StartAsync();

        //    // Create an HttpClient which is setup for the test host
        //    var client = host.GetTestClient();

        //    // Act
        //    //var response = await client.GetAsync("/Home/Test");
        //    var response = await client.GetAsync("weatherforecast");

        //    // Assert
        //    //var responseString = await response.Content.ReadAsStringAsync();
        //    //responseString.Should().Be("This is a test");

        //    var responseString = await response.Content.ReadAsStringAsync();
        //    var items = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(responseString);
        //    items.Any().Should().BeTrue();
        //}

        //[Fact]
        //public async Task BasicEndPointTest()
        //{
        //    // Arrange
        //    //var factory = new WebApplicationFactory<Asp31AuthWebApp.Startup>();
        //    var masterFactory = new WebApplicationFactory<Asp31AuthWebApp.Startup>();
        //    var factory = masterFactory.WithWebHostBuilder(builder =>
        //    {
        //        builder.ConfigureTestServices(
        //            services =>
        //            {
        //                //services.SwapTransient<IMySimpleGithubClient, MockSimpleGithubClient>();
        //            });
        //    });

        //    // Create an HttpClient which is setup for the test host
        //    var client = factory.CreateClient();

        //    // Act
        //    var response = await client.GetAsync("weatherforecast");

        //    // Assert
        //    var responseString = await response.Content.ReadAsStringAsync();
        //    var items = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(responseString);
        //    items.Any().Should().BeTrue();
        //}

        [Fact]
        public async Task Test3()
        {
            // Arrange
            var factory = new TestApplicationFactory<Asp31AuthWebApp.Startup>();

            // setup the swaps
            factory.Registrations = services =>
            {
                //services.SwapTransient<IMySimpleGithubClient, MockSimpleGithubClient>();
            };

            // Create an HttpClient which is setup for the test host
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("weatherforecast");

            // Assert
            //var responseString = await response.Content.ReadAsStringAsync();
            //var items = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(responseString);
            //items.Any().Should().BeTrue();

            var responseString = await response.Content.ReadAsStringAsync();

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
