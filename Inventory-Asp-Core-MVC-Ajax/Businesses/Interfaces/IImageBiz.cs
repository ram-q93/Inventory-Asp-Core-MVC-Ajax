using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface IImageBiz
    {
        Task<Result> AddImages(IList<ImageModel> imageModels);
        Task<Result<ImageModel>> GetById(int id);
        Task<Result> Delete(int id);
        Result<ImageModel> CreateImageModel(IFormFile file);
    }
}
