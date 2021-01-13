using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class ImageBiz : IImageBiz
    {
        public ImageBiz()
        {
        }

        #region CreateImage

        public Result<Image> CreateImage(IFormFile file) =>
            Result<Image>.Try(() =>
           {
               if (file == null)
               {
                   return Result<Image>.Successful();
               }

               MemoryStream ms = new MemoryStream();
               file.CopyTo(ms);
               var image = new Image()
               {
                   Data = ms.ToArray()
               };
               ms.Close();
               ms.Dispose();

               return Result<Image>.Successful(image);
           });

        #endregion

    }
}
