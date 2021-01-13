using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Profiles
{
    public class ProductDetailsProfile : Profile
    {
        public ProductDetailsProfile()
        {
            CreateMap<Product, ProductDetailsModel>();
                 //.ForMember(d => d.CategoryModel, opt => opt.MapFrom(s => s.Category))
                 //.ForMember(d => d.SupplierModel, opt => opt.MapFrom(s => s.Supplier))
                 //.ForMember(d => d.StorageModel, opt => opt.MapFrom(s => s.Storage))
                 //.ForMember(d => d.ImageModel, opt => opt.MapFrom(s => s.Image));
        }
    }
}
