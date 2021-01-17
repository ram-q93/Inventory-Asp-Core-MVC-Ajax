using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface IProductBiz
    {
        Task<Result> Add(ProductModel model);
        Task<Result> Delete(int id);
        Task<Result> Edit(ProductModel model);
        Task<Result<ProductModel>> GetById(int id);
        Task<Result<object>> List(DataTableParameters dtParameters);
        Task<Result<bool>> IsNameInUse(string name, int? id = null);
        Task<Result<ProductDetailsModel>> Details(int id);
    }
}