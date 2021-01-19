
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess
{
    public class InventoryDbContext : IdentityDbContext<User>, IInventoryDbContext
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

        public DbSet<Storage> Storages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Business> Businesses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Add uniqueness constraint
            modelBuilder.Entity<Storage>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Supplier>().HasIndex(s => s.CompanyName).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Business>().HasIndex(b => b.Name).IsUnique();

            modelBuilder.Entity<Business>()
               .HasMany(v => v.Addresses)
               .WithOne(a => a.Business)
               .HasForeignKey(a => a.BusinessId)
               .OnDelete(DeleteBehavior.Cascade); ;

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

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

    public interface IInventoryDbContext
    {
        DbSet<Storage> Storages { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<Address> Addresses { get; set; }
        public DbSet<Business> Businesses { get; set; }
    }
}
