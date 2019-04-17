using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApi.Models;

namespace WebApi.Components.Extension
{
    public static class HelperConfigExtension
    {
        private static HelperConfig helperConfig;

        public static void AddHelperConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HelperConfig>(configuration.GetSection("HelperConfig"));
            var provider = services.BuildServiceProvider();
            var settings = provider.GetRequiredService<IOptions<HelperConfig>>();

            helperConfig = settings.Value;
        }

        public static HelperConfig GetConfig()
        {
            return helperConfig;
        }
    }
}
