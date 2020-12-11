using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Profiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Supplier, SupplierModel>();
            CreateMap<SupplierModel, Supplier>();
        }
    }
}
