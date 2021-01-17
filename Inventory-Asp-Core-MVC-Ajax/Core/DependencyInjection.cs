using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Core.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory_Asp_Core_MVC_Ajax.Core
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddCoreLayer(this IServiceCollection services)
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
