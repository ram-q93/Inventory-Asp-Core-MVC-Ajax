using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AspNetCore.Lib.Services.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Common;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class DashboardBiz : IDashboardBiz
    {
        private readonly IRepository _repository;
        private readonly ISeedData _seedData;

        public DashboardBiz(IRepository repository, ISeedData seedData)
        {
            _repository = repository;
            _seedData = seedData;
        }

        public Task<Result<DashboardModel>> Statistics() => Result<DashboardModel>.TryAsync(async () =>
        {
            await _seedData.LoadSampleData();


            return Result<DashboardModel>.Successful(new DashboardModel()
            {
                TotalStorages = (await _repository.CountAllAsync<Storage>()).Data,
                TotalSuppliers = (await _repository.CountAllAsync<Supplier>(s => s.Enabled)).Data,
                TotalProducts = (await _repository.CountAllAsync<Product>()).Data
            });
        });


    }
}
