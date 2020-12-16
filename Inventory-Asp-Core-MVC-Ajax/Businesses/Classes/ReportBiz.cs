using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class ReportBiz
    {
        private readonly IRepository repository;

        public ReportBiz(IRepository repository)
        {
            this.repository = repository;
        }

        public Task<Result> GenerateReport(ProductReportModel model) =>
            Result.TryAsync(async () =>
            {
                var result = await repository.ListAsNoTrackingAsync<Product>(p =>
                    (model.IsAvailable != null && p.IsAvailable == model.IsAvailable) &&
                    (model.MaxPrice != null && p.Price <= model.MaxPrice) &&
                    (model.MinPrice != null && p.Price >= model.MinPrice) &&
                    (model.MaxQuantity != null && p.Quantity <= model.MaxQuantity) &&
                    (model.MinQuantity != null && p.Quantity >= model.MinQuantity) &&
                    (model.MaxPrice != null && p.Price <= model.MaxPrice) &&
                    (model.StorageId != null && p.StorageId == model.StorageId) &&
                    (model.SupplierId != null && p.SupplierId == model.SupplierId),
                    p => p.Storage, p => p.Supplier);
                if (!result.Success)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.ErrorInProductReport));
                }


                return Result.Successful();
            });
    }
}
