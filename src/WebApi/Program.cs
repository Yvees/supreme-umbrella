using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseKestrel(options =>
            {
                if (args.Any(g => g.Trim().ToLower().Contains("-port=")))
                {
                    string arg = args.SingleOrDefault(g => g.Trim().ToLower().Contains("-port="));
                    string sPort = arg.ToLower().Trim().Replace("-port=", string.Empty);
                    if (Int32.TryParse(sPort, out int port))
                    {
                        options.Listen(IPAddress.Any, port);
                    }
                }
                else
                    options.Listen(IPAddress.Any, 5080);
            });
    }
}
