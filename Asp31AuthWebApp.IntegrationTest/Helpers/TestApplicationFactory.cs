using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Asp31AuthWebApp.IntegrationTest
{
    //public class TestApplicationFactory</* TStartup, */ TTestStartup> : WebApplicationFactory<TTestStartup> where TTestStartup : class
    //{
    //    protected override IHostBuilder CreateHostBuilder()
    //    {
    //        var host = Host.CreateDefaultBuilder()
    //                        .ConfigureWebHost(builder =>
    //                        {
    //                            builder.UseStartup<TTestStartup>();
    //                        })
    //                        .ConfigureAppConfiguration((context, conf) =>
    //                        {
    //                            var projectDir = Directory.GetCurrentDirectory();
    //                            var configPath = Path.Combine(projectDir, "appsettings.json");

    //                            conf.AddJsonFile(configPath);
    //                        });

    //        return host;
    //    }

    //    protected override void ConfigureWebHost(IWebHostBuilder builder)
    //    {

    //    }

    //}

    public class TestApplicationFactory</* TStartup, */ TTestStartup> : WebApplicationFactory<TTestStartup> where TTestStartup : class
    {
        public Action<IServiceCollection> Registrations
        {
            get; set;
        }

        public TestApplicationFactory() : this(null)
        {
        }

        public TestApplicationFactory(Action<IServiceCollection> registrations = null)
        {
            Registrations = registrations ?? (collection => { });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                Registrations?.Invoke(services);
            });
        }
    }
}
