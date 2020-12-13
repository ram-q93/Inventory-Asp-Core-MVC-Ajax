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
        //static string Ip { get; set; }
        //static int Port { get; set; }
        public static void Main(string[] args)
        {
           // CreateHostBuilder(args).Build().Run();
            Statistics.WebHost = CreateWebHostBuilder(args).Build();
            Statistics.WebHost.Run();

        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //  Host.CreateDefaultBuilder(args)
        //      .ConfigureWebHostDefaults(webBuilder =>
        //      {
        //          webBuilder.UseStartup<Startup>();
        //      });

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>();



        //public static void Main(string[] args)
        //{
        //    var config = new ConfigurationBuilder().AddCommandLine(args).Build();
        //    Ip = config.GetValue<string>("ip") ?? "0.0.0.0";
        //    Port = config.GetValue<int?>("port") ?? 6006;
        //    var httpsPort = config.GetValue<int?>("https") ?? 5005;
        //    Statistics.WebHost = CreateWebHostBuilder(args).Build();
        //    Statistics.WebHost.Run();
        //}
 
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //   WebHost.CreateDefaultBuilder(args)
        //       .UseStartup<Startup>()
        //   .UseKestrel(options =>
        //   {
        //       options.Listen(IPAddress.Parse(Ip), Port);
        //   });


      
    }
}
