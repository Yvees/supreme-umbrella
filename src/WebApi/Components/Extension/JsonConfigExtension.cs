using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SystemCommonLibrary.Json;
using WebApi.Models;

namespace WebApi.Components.Extension
{
    public static class JsonConfigExtension
    {
        public static void AddJsonConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HelperConfig>(configuration.GetSection("HelperConfig"));
            var provider = services.BuildServiceProvider();
            var settings = provider.GetRequiredService<IOptions<HelperConfig>>();

            HelperConfig.Current = settings.Value;
            WxMenu.Current = File.ReadAllText("wxmenu.json");
        }
    }
}
