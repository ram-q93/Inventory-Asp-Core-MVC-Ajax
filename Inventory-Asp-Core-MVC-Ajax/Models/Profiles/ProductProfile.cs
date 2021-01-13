using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;

namespace InventoryProject.Model.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductModel>()
                //.ForMember(pm=>pm.CategoryName ,p=>p.);
                .ForMember(d => d.SupplierCompanyName, opt =>
                    opt.MapFrom(s => s.Supplier != null ? s.Supplier.CompanyName : null))
                .ForMember(d => d.StorageName, opt =>
                    opt.MapFrom(s => s.Storage != null ? s.Storage.Name : null))
                .ForMember(d => d.CategoryName, opt =>
                    opt.MapFrom(s => s.Category != null ? s.Category.Name : null));

            CreateMap<ProductModel, Product>();
        }
    }
}
