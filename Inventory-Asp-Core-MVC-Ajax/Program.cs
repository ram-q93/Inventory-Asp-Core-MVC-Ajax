using AspNetCore.Lib.Configurations;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.Commons;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Inventory_Asp_Core_MVC_Ajax
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Statistics.WebHost = CreateWebHostBuilder(args).Build().MigrateDatabase();
            Statistics.WebHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

    }
}
