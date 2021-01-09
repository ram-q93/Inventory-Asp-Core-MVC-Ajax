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
        Task<Result<SupplierModel>> GetById(int id);
        Task<Result<object>> List(DataTableParameters dtParameters);
        Task<Result<bool>> IsNameInUse(string name, int? id = null);
    }
}