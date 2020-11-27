using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Api;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using Inventory_Asp_Core_MVC_Ajax.Models.Profiles;
using InventoryProject.Model.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Inventory_Asp_Core_MVC_Ajax
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddServicesToContainer(services);
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=storage}/{action=storages}/{id?}");
            });
        }


        private void AddServicesToContainer(IServiceCollection services)
        {
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddScoped<IRepository, Repository<InventoryDbContext>>();

            //services.AddDbContext<IdentityAppDbContext>(options =>
            //  options.UseSqlServer(Configuration.GetConnectionString("Default")));
            //services.AddIdentity<DAL.EFModels.User, Role>(options =>
            //{
            //    options.User.RequireUniqueEmail = true;
            //    options.Password.RequiredLength = 4;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireDigit = false;
            //}
            //).AddEntityFrameworkStores<IdentityAppDbContext>();

            // services.Scan(scan => scan.FromCallingAssembly().AddClasses().AsMatchingInterface());
            //services.Scan(scan => scan
            //.FromCallingAssembly()
            // .FromApplicationDependencies(a => a.FullName.Contains("InventoryProject"))
            //  .AddClasses(publicOnly: true)
            //.AsMatchingInterface((service, filter) => filter.Where(implementation =>
            //       implementation.Name.Equals($"I{service.Name}", StringComparison.OrdinalIgnoreCase)))
            //.WithScopedLifetime());

            services.AddAutoMapper(typeof(StorageProfile), typeof(ProductProfile), typeof(ImageProfile));
            new ConfigureServices().AddServices(services);

        }
    }
}
