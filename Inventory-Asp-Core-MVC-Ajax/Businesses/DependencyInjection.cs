using AspNetCore.Lib.Configurations;
using AspNetCore.Lib.Enums;
using DinkToPdf;
using DinkToPdf.Contracts;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBussinessLayer(this IServiceCollection services)
        {
            services.ConfigCultureRequest();

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            var resultServices = TypeRegister
              .ScanAssemblyTypes(Assembly.GetExecutingAssembly())
              .Concat(new AspNetCore.Lib.Configurations.LayerServicesTypes().GetServices(null))
              .ToList();

            resultServices.GroupBy(s => s.Lifetime).ToList().ForEach(g =>
            {
                if (g.Key == TypeLifetime.Transient)
                    g.ToList().ForEach(s =>
                    {
                        if (s.Implement != null)
                            services.AddTransient(s.BaseType, s.Implement);
                        else
                            services.AddTransient(s.BaseType, s.ImplementationType);
                    });

                else if (g.Key == TypeLifetime.Scoped)
                    g.ToList().ForEach(s =>
                    {
                        if (s.Implement != null)
                            services.AddScoped(s.BaseType, s.Implement);
                        else
                            services.AddScoped(s.BaseType, s.ImplementationType);
                    });

                else if (g.Key == TypeLifetime.Singleton)
                    g.ToList().ForEach(s =>
                    {
                        if (s.Implement != null)
                            services.AddSingleton(s.BaseType, s.Implement);
                        else if (s.Instance != null && s.BaseType != null)
                            services.AddSingleton(s.BaseType, s.Instance);
                        else if (s.Instance != null && s.BaseType == null)
                            services.AddSingleton(s.Instance);
                        else if (s.BaseType == null)
                            services.AddSingleton(s.ImplementationType);
                        else
                            services.AddSingleton(s.BaseType, s.ImplementationType);
                    });
            });


            return services;
        }
    }
}
