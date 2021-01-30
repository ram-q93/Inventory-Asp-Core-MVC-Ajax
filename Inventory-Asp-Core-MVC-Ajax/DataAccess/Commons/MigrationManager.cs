using AspNetCore.Lib.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess.Commons
{
    public static class MigrationManager
    {
        public static async Task<IWebHost> MigrateDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                {
                    try
                    {
                        //adding migrations
                        services.GetRequiredService<InventoryDbContext>().Database.Migrate();

                        //seeding sample data
                        await services.GetRequiredService<ISampleDataSeeder>().SeedAllAsync(CancellationToken.None);

                    }
                    catch (Exception ex)
                    {
                        services.GetRequiredService<ILogger>().Exception(ex);
                    }
                }
            }

            return host;
        }
    }
}
