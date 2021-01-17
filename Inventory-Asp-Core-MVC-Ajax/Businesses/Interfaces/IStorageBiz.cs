using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using System.Threading.Tasks;

namespace InventoryProject.Business.Interfaces
{
    public interface IStorageBiz
    {
        Task<Result> Add(StorageModel model);
        Task<Result> Delete(int id);
        Task<Result> Edit(StorageModel model);
        Task<Result<StorageModel>> GetById(int id);
        Task<Result<object>> List(DataTableParameters dtParameters);
        Task<Result<bool>> IsNameInUse(string name, int? id = null);

        Result<object> ListName();

    }
}