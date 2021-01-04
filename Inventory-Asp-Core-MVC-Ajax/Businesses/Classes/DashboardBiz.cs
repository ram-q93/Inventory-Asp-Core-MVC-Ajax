using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class DashboardBiz : IDashboardBiz
    {
        private readonly IRepository _repository;

        public DashboardBiz(IRepository repository)
        {
            _repository = repository;
        }

        public Task<Result<DashboardModel>> Statistics() => Result<DashboardModel>.TryAsync(async () =>
             Result<DashboardModel>.Successful(
                 new DashboardModel()
                 {
                     TotalStorages = (await _repository.CountAllAsync<Storage>()).Data,
                     TotalSuppliers = (await _repository.CountAllAsync<Supplier>(s => s.Enabled)).Data,
                     TotalProducts = (await _repository.CountAllAsync<Product>(s => s.IsAvailable)).Data
                 }));


    }
}
