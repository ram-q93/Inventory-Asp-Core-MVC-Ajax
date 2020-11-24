using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
namespace InventoryProject.Model.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageModel>();
            CreateMap<ImageModel, Image>();
        }
    }
}
