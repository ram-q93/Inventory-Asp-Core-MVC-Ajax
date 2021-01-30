using AspNetCore.Lib.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess.Commons
{
    public static class MigrationManager
    {
        public static IWebHost MigrateDatabase(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        scope.ServiceProvider.GetRequiredService<ILogger>().Exception(ex);
                    }
                }
            }

            return host;
        }
    }
}
