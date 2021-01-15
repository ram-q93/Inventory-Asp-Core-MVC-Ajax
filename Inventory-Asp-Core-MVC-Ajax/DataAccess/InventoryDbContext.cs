
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AspNetCore.Lib.Services.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess
{
    public class InventoryDbContext : DbContext, IInventoryDbContext
    {
        private readonly IMachineDateTime _dateTime;

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options, IMachineDateTime dateTime)
           : base(options)
        {
            _dateTime = dateTime;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add uniqueness constraint
            modelBuilder.Entity<Storage>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Supplier>().HasIndex(s => s.CompanyName).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(s => s.Name).IsUnique();
        }

        public DbSet<Storage> Storages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "ramin";
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = "q";
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChanges();
        }


    }
}
