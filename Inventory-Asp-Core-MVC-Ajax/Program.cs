using AspNetCore.Lib.Configurations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Inventory_Asp_Core_MVC_Ajax
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Statistics.WebHost = CreateWebHostBuilder(args).Build();
            Statistics.WebHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

    }
}
