using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;


namespace Inventory_Asp_Core_MVC_Ajax.Core.Profiles
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
