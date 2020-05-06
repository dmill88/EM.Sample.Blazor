using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using EM.Sample.Blazor.WebUI.Services;

namespace EM.Sample.Blazor.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            IServiceCollection services = builder.Services;

            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            System.Diagnostics.Debug.WriteLine("builder.HostEnvironment.BaseAddress " + builder.HostEnvironment.BaseAddress);
            services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(Settings.UriBlogCommandApi) });

            services.AddTransient<IBlogQueryService>(sp => new BlogQueryService(new HttpClient { BaseAddress = new Uri(Settings.UriBlogQueryApi) }) );
            services.AddTransient<IBlogCommandService>(sp => new BlogCommandService(new HttpClient { BaseAddress = new Uri(Settings.UriBlogCommandApi) }) );

            services.AddDevExpressBlazor();

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}
