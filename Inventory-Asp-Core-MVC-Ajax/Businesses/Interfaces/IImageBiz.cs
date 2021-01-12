using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Microsoft.AspNetCore.Http;


namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface IImageBiz
    {
        //Task<Result> AddImages(IList<ImageModel> imageModels);
        //Task<Result<ImageModel>> GetById(int id);
        //Task<Result> Delete(int id);
        Result<Image> CreateImage(IFormFile file);
    }
}
