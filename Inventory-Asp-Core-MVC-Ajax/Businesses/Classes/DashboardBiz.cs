using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System.Linq;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class DashboardBiz : IDashboardBiz
    {
        private readonly IRepository _repository;

        public DashboardBiz(IRepository repository)
        {
            _repository = repository;
        }

        public Result<DashboardModel> Statistics() => Result<DashboardModel>.Try(() =>
             Result<DashboardModel>.Successful(new DashboardModel()
             {
                 TotalStorages = _repository.GetCurrentContext().Set<Storage>().Count(),
                 TotalSuppliers = _repository.GetCurrentContext().Set<Supplier>().Count(s => s.Enabled),
                 TotalProducts = _repository.GetCurrentContext().Set<Product>().Count(p => p.IsAvailable),
             }));


    }
}
