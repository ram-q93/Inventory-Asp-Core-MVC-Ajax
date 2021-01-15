
using AspNetCore.Lib.Services;
using AspNetCore.Lib.Services.Classes;
using AspNetCore.Lib.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddScoped<IRepository, Repository<InventoryDbContext>>();

            return services;
        }
    }
}
