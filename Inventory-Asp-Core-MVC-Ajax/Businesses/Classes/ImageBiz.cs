using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class ImageBiz : IImageBiz
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public ImageBiz(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        //#region AddImages

        //public async Task<Result> AddImages(IList<ImageModel> imageModels)
        //{
        //    var images = imageModels.Select(i => mapper.Map<ImageModel, Image>(i)).ToList();
        //    images.ForEach(i => repository.Add(i));
        //    await repository.CommitAsync();
        //    return Result.Successful();
        //}

        //#endregion

        //#region GetById

        //public async Task<Result<ImageModel>> GetById(int id)
        //{
        //    var result = await repository.FirstOrDefaultAsNoTrackingAsync<Image>(p => p.Id == id);
        //    if (result?.Success != true || result?.Data == null)
        //    {
        //        return Result<ImageModel>.Failed(Error.WithCode(ErrorCodes.ImageNotFoundById));
        //    }
        //    return Result<ImageModel>.Successful(mapper.Map<Image, ImageModel>(result.Data));
        //}

        //#endregion

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

        //#region Delete

        //public async Task<Result> Delete(int id)
        //{
        //    var result = await GetById(id);
        //    if (!result.Success || result?.Data == null)
        //    {
        //        return Result.Failed(result.Error);
        //    }
        //    repository.Remove(mapper.Map<ImageModel, Image>(result.Data));
        //    await repository.CommitAsync();
        //    return Result.Successful();
        //}

        //#endregion
    }
}
