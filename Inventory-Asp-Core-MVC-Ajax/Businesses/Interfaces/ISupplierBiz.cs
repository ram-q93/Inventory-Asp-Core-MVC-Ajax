using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface ISupplierBiz
    {
        Task<Result> Add(SupplierModel model);
        Task<Result> Delete(int id);
        Task<Result> Edit(SupplierModel model);
        Task<Result<SupplierFilterModel>> GetSupplierPagedListFilteredBySearchQuery(int? page, string searchQuery);
        Task<Result<SupplierModel>> GetById(int id);
        Task<Result<SupplierModel>> Details(int id);
        Task<Result<object>> ListEnableSuppliers();
    }
}