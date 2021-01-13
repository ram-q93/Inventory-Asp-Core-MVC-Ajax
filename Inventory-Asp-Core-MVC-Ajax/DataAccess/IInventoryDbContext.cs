using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess
{
    public interface IInventoryDbContext
    {
        DbSet<Storage> Storages { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Image> Images { get; set; }
    }
}
