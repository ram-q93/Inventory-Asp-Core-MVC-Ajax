using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Microsoft.AspNetCore.Http;


namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface IImageBiz
    {
        Result<Image> CreateImage(IFormFile file);
    }
}
