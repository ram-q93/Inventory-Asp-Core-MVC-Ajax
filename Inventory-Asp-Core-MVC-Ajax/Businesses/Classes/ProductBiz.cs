using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
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
        public ProductBiz(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;

        }

        #region List

        public async Task<Result<IList<ProductModel>>> List()
        {
            var result = await repository.ListAsNoTrackingAsync<Product>();
            if (!result.Success)
            {
                return Result<IList<ProductModel>>.Failed(Error.WithCode(ErrorCodes.ProductsNotFound));
            }
            return Result<IList<ProductModel>>.Successful(
                result.Data.Select(product => mapper.Map<Product, ProductModel>(product)).ToList());
        }

        #endregion

        #region GetPagedListFilteredBySearchQuery

        public async Task<ResultList<ProductModel>> GetStoragePagedListProductFilteredBySearchQuery(int storageId, int? page, string searchQuery)
        {
            var resultList = await repository.ListAsNoTrackingAsync<Product>(p => p.StorageId == storageId &&
                searchQuery == null ||
                (p.Name != null && p.Name.Contains(searchQuery))
                //||
                //(s. != null && s.Phone.Contains(searchQuery)) ||
                //(s.Address != null && s.Address.Contains(searchQuery))
                ,
                new PagingModel()
                {
                    PageNumber = (page == null || page <= 0 ? 1 : page.Value) - 1,
                    PageSize = 5,
                    Sort = "UpdatedDate",
                    SortDirection = SortDirection.DESC
                },
                "UpdatedDate");

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
        }

        #endregion

        #region GetById

        public async Task<Result<ProductModel>> GetById(int id)
        {
            var result = await repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == id);
            if (result?.Success != true || result?.Data == null)
            {
                return Result<ProductModel>.Failed(Error.WithCode(ErrorCodes.ProductNotFoundById));
            }
            return Result<ProductModel>.Successful(mapper.Map<Product, ProductModel>(result.Data));
        }

        #endregion

        #region Add

        public async Task<Result> Add(ProductModel productModel)
        {
            var product = mapper.Map<ProductModel, Product>(productModel);
            product.Images = productModel.ImageModels.Select(i => mapper.Map<ImageModel, Image>(i)).ToList();
            product.CreatedDate = DateTime.Now;
            product.UpdatedDate = DateTime.Now;
            repository.Add(product);
            await repository.CommitAsync();
            return Result.Successful();
        }

        #endregion

        #region Edit
        public async Task<Result> Edit(ProductModel productModel)
        {
            var result = await repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == productModel.Id);
            if (result?.Success != true || result?.Data == null)
            {
                return Result.Failed(Error.WithCode(ErrorCodes.ProductNotFoundById));
            }
            var product = mapper.Map<ProductModel, Product>(productModel);
            product.StorageId = result.Data.StorageId;
            product.UpdatedDate = DateTime.Now;
            repository.Update(product);
            await repository.CommitAsync();
            return Result.Successful();
        }

        #endregion

        #region Delete

        public async Task<Result> Delete(int id)
        {
            var result = await repository.FirstOrDefaultAsync<Product>(p => p.Id == id,
                includes: p => p.Images);
            if (!result.Success || result?.Data == null)
            {
                return Result.Failed(result.Error);
            }
            result.Data.Images.Clear();
            repository.Remove(result.Data);
            await repository.CommitAsync();
            return Result.Successful();
        }

        #endregion

        #region StorageJoinedToProductListByStoreId

        public async Task<Result<StorageModel>> StorageJoinedToProductListByStoreId(int storeId)
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
        }

        #endregion

        #region Details

        public async Task<Result<ProductModel>> Details(int id)
        {
            var storageResult = await repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == id,
                p => p.Images, p => p.Storage);
            if (!storageResult.Success)
            {
                return Result<ProductModel>.Failed(Error.WithCode(ErrorCodes.ProductDetailsNotFoundById));
            }
            var productModel = mapper.Map<Product, ProductModel>(storageResult.Data);
            productModel.StorageModel = mapper.Map<Storage, StorageModel>(storageResult.Data.Storage);
            productModel.ImageModels = storageResult.Data.Images.Select(i => new ImageModel()
            {
                Id = i.Id,
                Title = i.Title,
                ConvertedData = Convert.ToBase64String(i.Data),
                ProductId = i.productId
            }).ToList();
            return Result<ProductModel>.Successful(productModel);
        }

        #endregion


    }
}
