
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add uniqueness constraint
            modelBuilder.Entity<Storage>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(s => s.Name).IsUnique();
        }

        public DbSet<Storage> Storages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
