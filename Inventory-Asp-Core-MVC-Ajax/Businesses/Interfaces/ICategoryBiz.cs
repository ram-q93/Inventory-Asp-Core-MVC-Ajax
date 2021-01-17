using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface ICategoryBiz
    {
        Task<Result<object>> List(DataTableParameters dtParameters);
        Task<Result> Add(CategoryModel model);
        Task<Result> Delete(int id);
        Task<Result> Edit(CategoryModel model);
        Task<Result<CategoryModel>> GetById(int id);
        Task<Result<bool>> IsNameInUse(string name, int? id = null);
        Result<object> ListName();
    }
}
