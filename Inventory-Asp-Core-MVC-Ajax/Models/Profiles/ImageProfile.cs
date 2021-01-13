using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Profiles
{
    internal class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageModel>();
            CreateMap<ImageModel, Image>();
        }
    }
}