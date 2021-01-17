using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;

namespace Inventory_Asp_Core_MVC_Ajax.Core.Profiles
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