using AutoMapper;
using Helper.Library.Models;
using Helper.Library.Services;
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
        public ProductBiz(IRepository repository,
                               IMapper mapper)
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

    }
}
