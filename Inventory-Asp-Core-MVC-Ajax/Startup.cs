using Inventory_Asp_Core_MVC_Ajax.Api;
using Inventory_Asp_Core_MVC_Ajax.Api.Middlewares;
using Inventory_Asp_Core_MVC_Ajax.Businesses;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Inventory_Asp_Core_MVC_Ajax
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiLayer();
            services.AddBussinessLayer();
            services.AddDataAccessLayer(Configuration);

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers().AddNewtonsoftJson();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error/500");
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseMiddleware<RequestPerformanceBehaviourMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMiddleware<RequestErrorHandlerMiddleware>();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=dashboard}/{action=statistics}/{id?}");
            });
        }
    }
}
