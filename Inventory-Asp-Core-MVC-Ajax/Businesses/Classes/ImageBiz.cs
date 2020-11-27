using AutoMapper;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System.Collections.Generic;
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

        #region Get Store, Products And Images

        public async Task<Result<StorageModel>> GetStoreProductsAndImages(int productId)
        {
            var productAndStorageResult = await repository.FirstOrDefaultAsNoTrackingAsync<Product>(p =>
                p.Id == productId, includes: p => p.Storage);
            if (!productAndStorageResult.Success || productAndStorageResult.Data == null)
            {
                return Result<StorageModel>.Failed(
                    Error.WithCode(ErrorCodes.ProductJoinedWithStoreNotFoundByProductId));
            }
            var imagesResult = await repository.ListAsNoTrackingAsync<Image>(i => i.productId == productId);
            if (!imagesResult.Success)
            {
                return Result<StorageModel>.Failed(Error.WithCode(ErrorCodes.ImagesNotFoundByProductId));
            }
            var storeModel = mapper.Map<Storage, StorageModel>(productAndStorageResult.Data.Storage);
            storeModel.ProductModels = new List<ProductModel>() {
                mapper.Map<Product, ProductModel>(productAndStorageResult.Data) };
            storeModel.ProductModels.First().ImageModels =
                imagesResult.Data.Select(i => mapper.Map<Image, ImageModel>(i)).ToList();
            return Result<StorageModel>.Successful(storeModel);
        }

        #endregion

        #region AddImages

        public async Task<Result> AddImages(IList<ImageModel> imageModels)
        {
            var images = imageModels.Select(i => mapper.Map<ImageModel, Image>(i)).ToList();
            images.ForEach(i => repository.Add(i));
            await repository.CommitAsync();
            return Result.Successful();
        }

        #endregion

        #region GetById

        public async Task<Result<ImageModel>> GetById(int id)
        {
            var result = await repository.FirstOrDefaultAsNoTrackingAsync<Image>(p => p.Id == id);
            if (result?.Success != true || result?.Data == null)
            {
                return Result<ImageModel>.Failed(Error.WithCode(ErrorCodes.ImageNotFoundById));
            }
            return Result<ImageModel>.Successful(mapper.Map<Image, ImageModel>(result.Data));
        }

        #endregion

        #region Delete

        public async Task<Result> Delete(int id)
        {
            var result = await GetById(id);
            if (!result.Success || result?.Data == null)
            {
                return Result.Failed(result.Error);
            }
            repository.Remove(mapper.Map<ImageModel, Image>(result.Data));
            await repository.CommitAsync();
            return Result.Successful();
        }

        #endregion
    }
}
