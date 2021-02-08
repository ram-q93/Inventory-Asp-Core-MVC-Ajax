using AspNetCore.Lib.Services.Classes;
using AspNetCore.Lib.Services.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Microsoft.AspNetCore.Identity;
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
            //services.AddDbContext<InventoryDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("Default")));
            //services.AddScoped<IRepository, Repository<InventoryDbContext>>();


            services.AddDbContext<InventoryDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddScoped<IRepository, Repository<InventoryDbContext>>();

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 3;
            });


            return services;
        }
    }
}
