using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;


namespace Inventory_Asp_Core_MVC_Ajax.Models.Profiles
{
    public class StorageProfile : Profile
    {
        public StorageProfile()
        {
            CreateMap<Storage, StorageModel>();
            CreateMap<StorageModel, Storage>();
        }
    }
}
