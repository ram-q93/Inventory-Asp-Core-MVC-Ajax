using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
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

        public Task<Result<DashboardModel>> Statistics() => Result<DashboardModel>
            .TryAsync(async () => Result<DashboardModel>.Successful(new DashboardModel()
            {
                TotalStorages = (await _repository.CountAllAsync<Storage>()).Data,
                TotalSuppliers = (await _repository.CountAllAsync<Supplier>(s => s.Enabled)).Data,
                TotalProducts = (await _repository.CountAllAsync<Product>()).Data
            })
        );

    }
}
