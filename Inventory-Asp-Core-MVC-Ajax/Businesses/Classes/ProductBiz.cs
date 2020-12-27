using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class ProductBiz : IProductBiz
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly IImageBiz imageBiz;

        public ProductBiz(IRepository repository, IMapper mapper, IImageBiz imageBiz)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.imageBiz = imageBiz;
        }

        #region List

        public Task<Result<IList<ProductModel>>> List() =>
            Result<IList<ProductModel>>.TryAsync(async () =>
            {
                var result = await repository.ListAsNoTrackingAsync<Product>();
                if (!result.Success)
                {
                    return Result<IList<ProductModel>>.Failed(Error.WithCode(ErrorCodes.ProductsNotFound));
                }
                return Result<IList<ProductModel>>.Successful(
                    result.Data.Select(product => mapper.Map<Product, ProductModel>(product)).ToList());
            });

        #endregion

        #region GetStoragePagedListProductFilteredBySearchQuery

        public Task<ResultList<ProductModel>> GetStoragePagedListProductFilteredBySearchQuery(int storageId,
            int? page, string searchQuery) => ResultList<ProductModel>.TryAsync(async () =>
             {
                 var pagingModel = new PagingModel()
                 {
                     PageNumber = (page == null || page <= 0 ? 1 : page.Value) - 1,
                     PageSize = 5,
                     Sort = "LastModified",
                     SortDirection = SortDirection.DESC
                 };
                 var resultList = await repository.ListAsNoTrackingAsync<Product>(p => p.StorageId == storageId &&
                     searchQuery == null ||
                     (p.Name != null && p.Name.Contains(searchQuery)) ||
                     (p.Quantity < Convert.ToInt32(searchQuery)) ||
                     (p.Price < Convert.ToDecimal(searchQuery)),
                     pagingModel, "LastModified");

                 if (!resultList.Success)
                 {
                     return ResultList<ProductModel>.Failed(Error.WithCode(ErrorCodes.PagedListFilteredBySearchQueryNotFound));
                 }
                 return new ResultList<ProductModel>()
                 {
                     Success = true,
                     Items = resultList.Items.Select(p => mapper.Map<Product, ProductModel>(p)).ToList(),
                     PageNumber = resultList.PageNumber,
                     PageSize = resultList.PageSize,
                     TotalCount = resultList.TotalCount
                 };
             });

        #endregion

        #region GetById

        public Task<Result<ProductModel>> GetById(int id) =>
            Result<ProductModel>.TryAsync(async () =>
        {
            var result = await repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == id, p => p.Image);
            if (result?.Success != true || result?.Data == null)
            {
                return Result<ProductModel>.Failed(Error.WithCode(ErrorCodes.ProductNotFoundById));
            }
            var productModel = mapper.Map<Product, ProductModel>(result.Data);
            if (result.Data.Image != null)
            {
                productModel.ImageModel = new ImageModel()
                {
                    Id = result.Data.Image.Id,
                    Title = result.Data.Image.Title,
                    Caption = result.Data.Image.Caption,
                    ConvertedData = Convert.ToBase64String(result.Data.Image.Data)
                };
            }
            else
            {
                productModel.ImageModel = new ImageModel();
            }
            return Result<ProductModel>.Successful(productModel);
        });

        #endregion

        #region Add

        public Task<Result> Add(ProductModel productModel) =>
            Result.TryAsync(async () =>
            {
                if (!(await IsNameInUse(productModel.Name)).Data)
                {
                    Result.Failed(Error.WithCode(ErrorCodes.ProductNameAlreadyInUse));
                }

                productModel.ImageModel = imageBiz.CreateImageModel(productModel.ProductPicture).Data;

                var product = mapper.Map<ProductModel, Product>(productModel);
                product.Image = mapper.Map<ImageModel, Image>(productModel.ImageModel);

                repository.Add(product);
                await repository.CommitAsync();
                return Result.Successful();
            });

        #endregion

        #region Edit
        public Task<Result> Edit(ProductModel productModel) =>
            Result.TryAsync(async () =>
            {
                if (!(await IsNameInUse(productModel.Name)).Data)
                {
                    Result.Failed(Error.WithCode(ErrorCodes.ProductNameAlreadyInUse));
                }

                var result = await repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == productModel.Id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.ProductNotFoundById));
                }
                productModel.ImageModel = imageBiz.CreateImageModel(productModel.ProductPicture).Data;

                var product = mapper.Map<ProductModel, Product>(productModel);
                product.Image = mapper.Map<ImageModel, Image>(productModel.ImageModel);

                product.StorageId = result.Data.StorageId;
                repository.Update(product);
                await repository.CommitAsync();
                return Result.Successful();
            });

        #endregion

        #region Delete

        public Task<Result> Delete(int id) =>
            Result.TryAsync(async () =>
            {
                var result = await repository.FirstOrDefaultAsync<Product>(p => p.Id == id,
                    includes: p => p.Image);
                if (!result.Success || result?.Data == null)
                {
                    return Result.Failed(result.Error);
                }
                result.Data.Image = null;
                repository.Remove(result.Data);
                await repository.CommitAsync();
                return Result.Successful();
            });

        #endregion

        #region StorageJoinedToProductListByStoreId

        public Task<Result<StorageModel>> StorageJoinedToProductListByStoreId(int storeId) =>
            Result<StorageModel>.TryAsync(async () =>
            {
                var storageResult = await repository.FirstOrDefaultAsNoTrackingAsync<Storage>(s => s.Id == storeId,
                    includes: s => s.Products);
                if (!storageResult.Success)
                {
                    return Result<StorageModel>.Failed(Error.WithCode(ErrorCodes.ProductsNotFoundByStoreId));
                }
                var products = storageResult.Data.Products.Select(p => mapper.Map<Product, ProductModel>(p)).ToList();
                var store = mapper.Map<Storage, StorageModel>(storageResult.Data);
                store.ProductModels = products;
                return Result<StorageModel>.Successful(store);
            });

        #endregion

        #region Details

        public Task<Result<ProductModel>> Details(int id) =>
            Result<ProductModel>.TryAsync(async () =>
            {
                var productResult = await repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == id,
                    p => p.Image, p => p.Storage, p => p.Supplier);
                if (!productResult.Success)
                {
                    return Result<ProductModel>.Failed(Error.WithCode(ErrorCodes.ProductDetailsNotFoundById));
                }
                var productModel = mapper.Map<Product, ProductModel>(productResult.Data);
                productModel.StorageModel = mapper.Map<Storage, StorageModel>(productResult.Data.Storage);
                productModel.SupplierModel = mapper.Map<Supplier, SupplierModel>(productResult.Data.Supplier);
                if (productResult.Data.Image != null)
                {
                    productModel.ImageModel = new ImageModel()
                    {
                        Id = productResult.Data.Image.Id,
                        Title = productResult.Data.Image.Title,
                        Caption = productResult.Data.Image.Caption,
                        ConvertedData = Convert.ToBase64String(productResult.Data.Image.Data)
                    };
                }
                else
                {
                    productModel.ImageModel = new ImageModel();
                }
                return Result<ProductModel>.Successful(productModel);
            });

        #endregion

        #region IsNameInUse

        public Task<Result<bool>> IsNameInUse(string name) =>
            Result<bool>.TryAsync(async () =>
            {
                var result = await repository.FirstOrDefaultAsNoTrackingAsync<Product>(s => s.Name == name);
                return Result<bool>.Successful(result.Data == null);
            });

        #endregion

    }
}
