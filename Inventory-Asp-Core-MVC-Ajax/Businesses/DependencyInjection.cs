using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Models.Profiles;
using InventoryProject.Model.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBussinessLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(StorageProfile),
                typeof(ProductProfile),
                typeof(ImageProfile),
                typeof(SupplierProfile),
                typeof(ProductDetailsProfile),
                typeof(CategoryProfile));

            return services;
        }
    }
}
