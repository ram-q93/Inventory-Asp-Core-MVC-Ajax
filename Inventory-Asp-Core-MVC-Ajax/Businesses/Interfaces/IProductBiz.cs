using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface IProductBiz
    {
        Task<Result> Add(ProductModel productModel);
        Task<Result> Delete(int id);
        Task<Result> Edit(ProductModel productModel);
        Task<Result<ProductModel>> GetById(int id);
        Task<Result<IList<ProductModel>>> List();
        Task<Result<StorageModel>> StorageJoinedToProductListByStoreId(int storeId);
        Task<ResultList<ProductModel>> GetStoragePagedListProductFilteredBySearchQuery(int storageId, int? page, string searchQuery);
    }
}