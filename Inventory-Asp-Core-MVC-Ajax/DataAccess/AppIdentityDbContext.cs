//using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace Inventory_Asp_Core_MVC_Ajax.DataAccess
//{
//    public class AppIdentityDbContext : IdentityDbContext<User>
//    {
//        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
//        {
//        }

//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);
//            builder.Entity<Business>()
//                .HasMany(v => v.Addresses)
//                .WithOne(a => a.Business)
//                .HasForeignKey(a => a.BusinessId)
//                .OnDelete(DeleteBehavior.Cascade); ;

//            foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
//            {
//                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
//            }
//        }

//        public DbSet<Business> Businesses { get; set; }
//    }
//}
