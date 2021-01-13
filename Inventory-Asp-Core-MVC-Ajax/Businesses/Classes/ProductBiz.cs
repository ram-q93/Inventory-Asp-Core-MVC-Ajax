using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
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
        private readonly IRepository _repository;
        private readonly IImageBiz _imageBiz;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProductBiz(IRepository repository, IImageBiz imageBiz, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _imageBiz = imageBiz;
            _mapper = mapper;
            _logger = logger;
        }

        #region List

        public Task<Result<object>> List(DataTableParameters dtParameters) =>
            Result<object>.TryAsync(async () =>
            {
                var searchBy = dtParameters.Search?.Value;
                var orderCriteria = string.Empty;
                var orderAscendingDirection = true;
                if (dtParameters.Order != null)
                {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                }
                else
                {
                    orderCriteria = "Id";
                    orderAscendingDirection = true;
                }

                var pagingModel = new PagingModel()
                {
                    PageNumber = dtParameters.Start == 0 ? 0 : dtParameters.Start / dtParameters.Length,
                    PageSize = dtParameters.Length,
                    Sort = orderCriteria,
                    SortDirection = orderAscendingDirection ? SortDirection.ASC : SortDirection.DESC
                };

                var resultList = await _repository.SortedPageListAsNoTrackingAsync<Product>(p =>
                           searchBy == null ||
                           (p.Name != null && p.Name.Contains(searchBy)) ||
                           (p.Code != null && p.Code.Contains(searchBy)) ||
                           (p.Description != null && p.Description.Contains(searchBy)),
                            //(int.TryParse(searchBy, out q)  s.Quantity < q ) ||
                            //(s.UnitePrice < Convert.ToDecimal(searchBy)))
                            pagingModel,
                            p => p.Storage,
                            p => p.Supplier,
                            p => p.Category);

                if (!resultList.Success)
                {
                    return Result<object>.Failed(
                        Error.WithData(ErrorCodes.ProductsNotFound, new[] { "Some thing went wrong!" }));
                }

                var totalCount = (await _repository.CountAllAsync<Product>()).Data;
                var totalFilteredCount = resultList.TotalCount;
                var productModels = _mapper.Map<IEnumerable<ProductModel>>(resultList.Items);

                return Result<object>.Successful(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalFilteredCount,
                    data = productModels
                });
            });

        #endregion

        #region GetById

        public Task<Result<ProductModel>> GetById(int id) =>
            Result<ProductModel>.TryAsync(async () =>
            {
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == id,
                   p => p.Category,
                   p => p.Storage,
                   p => p.Supplier,
                   p => p.Image);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result<ProductModel>.Failed(Error.WithData(ErrorCodes.ProductNotFoundById,
                        new[] { "Product not found!" }));
                }

                var productModel = _mapper.Map<Product, ProductModel>(result.Data);
                if (result.Data.Image != null)
                {
                    productModel.ImageModel = _mapper.Map<Image, ImageModel>(result.Data.Image);
                }
                return Result<ProductModel>.Successful(productModel);
            });

        #endregion

        #region Add

        public Task<Result> Add(ProductModel model) =>
            Result.TryAsync(async () =>
            {
                if ((await IsNameInUse(model.Name)).Data)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.ProductNameAlreadyInUse,
                       new[] { "Product name already in use!" }));
                }

                var newProduct = _mapper.Map<ProductModel, Product>(model);
                newProduct.Image = _imageBiz.CreateImage(model.ProductPicture).Data;

                _repository.Add(newProduct);
                await _repository.CommitAsync();
                return Result.Successful();
            });

        #endregion

        #region Edit

        public Task<Result> Edit(ProductModel model) =>
            Result.TryAsync(async () =>
            {
                if ((await IsNameInUse(model.Name, model.Id)).Data)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.ProductNameAlreadyInUse,
                      new[] { "Product name already in use!" }));
                }

                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == model.Id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.ProductNotFoundById,
                        new[] { "Product not found!" }));
                }

                var product = result.Data;
                product.Name = model.Name;
                product.Code = model.Code;
                product.UnitePrice = model.UnitePrice;
                product.Quantity = model.Quantity;
                product.Enabled = model.Enabled;
                product.Description = model.Description;
                product.CategoryId = model.CategoryId;
                product.StorageId = model.StorageId;
                product.SupplierId = model.SupplierId;
                if (model.ProductPicture != null)
                {
                    product.Image.Id = Convert.ToInt32(model.ImageId);
                    product.Image = _imageBiz.CreateImage(model.ProductPicture).Data;
                }

                _repository.Update(product);
                await _repository.CommitAsync();
                return Result.Successful();
            });

        #endregion

        #region Delete

        public Task<Result> Delete(int id) =>
            Result.TryAsync(async () =>
            {
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == id);
                if (!result.Success || result?.Data == null)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.ProductNotFoundById,
                        new[] { "Product not found!" }));
                }

                var product = result.Data;
                if (product.ImageId != null)
                {
                    var imageResult = await _repository.FirstOrDefaultAsNoTrackingAsync<Image>(i =>
                     i.Id == product.ImageId);
                    _repository.Remove(imageResult?.Data);
                }

                _repository.Remove(product);
                await _repository.CommitAsync();
                _logger.Warn($"Product Deleted  {product.Name}");
                return Result.Successful();
            });

        #endregion

        #region IsNameInUse

        public Task<Result<bool>> IsNameInUse(string name, int? id = null) =>
            Result<bool>.TryAsync(async () =>
            {
                var result = await _repository.ExistsAsync<Product>(s => s.Name == name &&
                (id == null || s.Id != id));// check id for edit: 
                return Result<bool>.Successful(result.Data);
            });

        #endregion

        #region Details

        public Task<Result<ProductDetailsModel>> Details(int id) =>
            Result<ProductDetailsModel>.TryAsync(async () =>
            {
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == id,
                   p => p.Category,
                   p => p.Storage,
                   p => p.Supplier,
                   p => p.Image);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result<ProductDetailsModel>.Failed(Error.WithData(ErrorCodes.ProductNotFoundById,
                        new[] { "Product not found!" }));
                }

                var productDetailsModel = _mapper.Map<Product, ProductDetailsModel>(result.Data);
                if (result.Data.Category != null)
                    productDetailsModel.CategoryModel = _mapper.Map<Category, CategoryModel>(result.Data.Category);
                if (result.Data.Storage != null)
                    productDetailsModel.StorageModel = _mapper.Map<Storage, StorageModel>(result.Data.Storage);
                if (result.Data.Supplier != null)
                    productDetailsModel.SupplierModel = _mapper.Map<Supplier, SupplierModel>(result.Data.Supplier);
                if (result.Data.Image != null)
                    productDetailsModel.ImageModel = _mapper.Map<Image, ImageModel>(result.Data.Image);

                return Result<ProductDetailsModel>.Successful(productDetailsModel);
            });

        #endregion





        #region List

        public Task<Result<IList<ProductModel>>> List() =>
            Result<IList<ProductModel>>.TryAsync(async () =>
            {
                var result = await _repository.ListAsNoTrackingAsync<Product>();
                if (!result.Success)
                {
                    return Result<IList<ProductModel>>.Failed(Error.WithCode(ErrorCodes.ProductsNotFound));
                }
                return Result<IList<ProductModel>>.Successful(
                    result.Data.Select(product => _mapper.Map<Product, ProductModel>(product)).ToList());
            });

        #endregion


        //#region GetById

        //public Task<Result<ProductModel>> GetById(int id) =>
        //    Result<ProductModel>.TryAsync(async () =>
        //{
        //    var result = await repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == id, p => p.Image);
        //    if (result?.Success != true || result?.Data == null)
        //    {
        //        return Result<ProductModel>.Failed(Error.WithCode(ErrorCodes.ProductNotFoundById));
        //    }
        //    var productModel = mapper.Map<Product, ProductModel>(result.Data);
        //    if (result.Data.Image != null)
        //    {
        //        productModel.ImageModel = new ImageModel()
        //        {
        //            Id = result.Data.Image.Id,
        //            Title = result.Data.Image.Title,
        //            Caption = result.Data.Image.Caption,
        //            ConvertedData = Convert.ToBase64String(result.Data.Image.Data)
        //        };
        //    }
        //    else
        //    {
        //        productModel.ImageModel = new ImageModel();
        //    }
        //    return Result<ProductModel>.Successful(productModel);
        //});

        //#endregion

        //#region Add

        //public Task<Result> Add(ProductModel productModel) =>
        //    Result.TryAsync(async () =>
        //    {
        //        if (!(await IsNameInUse(productModel.Name)).Data)
        //        {
        //            Result.Failed(Error.WithCode(ErrorCodes.ProductNameAlreadyInUse));
        //        }

        //        productModel.ImageModel = imageBiz.CreateImageModel(productModel.ProductPicture).Data;

        //        var product = mapper.Map<ProductModel, Product>(productModel);
        //        product.Image = mapper.Map<ImageModel, Image>(productModel.ImageModel);

        //        repository.Add(product);
        //        await repository.CommitAsync();
        //        return Result.Successful();
        //    });

        //#endregion

        //#region Edit
        //public Task<Result> Edit(ProductModel productModel) =>
        //    Result.TryAsync(async () =>
        //    {
        //        if (!(await IsNameInUse(productModel.Name)).Data)
        //        {
        //            Result.Failed(Error.WithCode(ErrorCodes.ProductNameAlreadyInUse));
        //        }

        //        var result = await repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == productModel.Id);
        //        if (result?.Success != true || result?.Data == null)
        //        {
        //            return Result.Failed(Error.WithCode(ErrorCodes.ProductNotFoundById));
        //        }
        //        productModel.ImageModel = imageBiz.CreateImageModel(productModel.ProductPicture).Data;

        //        var product = mapper.Map<ProductModel, Product>(productModel);
        //        product.Image = mapper.Map<ImageModel, Image>(productModel.ImageModel);

        //        product.StorageId = result.Data.StorageId;
        //        repository.Update(product);
        //        await repository.CommitAsync();
        //        return Result.Successful();
        //    });

        //#endregion

        //#region Delete

        //public Task<Result> Delete(int id) =>
        //    Result.TryAsync(async () =>
        //    {
        //        var result = await repository.FirstOrDefaultAsync<Product>(p => p.Id == id,
        //            includes: p => p.Image);
        //        if (!result.Success || result?.Data == null)
        //        {
        //            return Result.Failed(result.Error);
        //        }
        //        result.Data.Image = null;
        //        repository.Remove(result.Data);
        //        await repository.CommitAsync();
        //        return Result.Successful();
        //    });

        //#endregion

        #region StorageJoinedToProductListByStoreId

        public Task<Result<StorageModel>> StorageJoinedToProductListByStoreId(int storeId) =>
            Result<StorageModel>.TryAsync(async () =>
            {
                var storageResult = await _repository.FirstOrDefaultAsNoTrackingAsync<Storage>(s => s.Id == storeId,
                    includes: s => s.Products);
                if (!storageResult.Success)
                {
                    return Result<StorageModel>.Failed(Error.WithCode(ErrorCodes.ProductsNotFoundByStoreId));
                }
                var products = storageResult.Data.Products.Select(p => _mapper.Map<Product, ProductModel>(p)).ToList();
                var store = _mapper.Map<Storage, StorageModel>(storageResult.Data);
                store.ProductModels = products;
                return Result<StorageModel>.Successful(store);
            });

        #endregion

        #region Details

        //public Task<Result<ProductModel>> Details(int id) =>
        //    Result<ProductModel>.TryAsync(async () =>
        //    {
        //        var productResult = await _repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == id,
        //            //p => p.Image,
        //            p => p.Storage, p => p.Supplier);
        //        if (!productResult.Success)
        //        {
        //            return Result<ProductModel>.Failed(Error.WithCode(ErrorCodes.ProductDetailsNotFoundById));
        //        }
        //        var productModel = _mapper.Map<Product, ProductModel>(productResult.Data);
        //        productModel.StorageModel = _mapper.Map<Storage, StorageModel>(productResult.Data.Storage);
        //        productModel.SupplierModel = _mapper.Map<Supplier, SupplierModel>(productResult.Data.Supplier);
        //if (productResult.Data.Image != null)
        //{
        //    productModel.ImageModel = new ImageModel()
        //    {
        //        Id = productResult.Data.Image.Id,
        //        Title = productResult.Data.Image.Title,
        //        Caption = productResult.Data.Image.Caption,
        //        ConvertedData = Convert.ToBase64String(productResult.Data.Image.Data)
        //    };
        //}
        //else
        //{
        //    productModel.ImageModel = new ImageModel();
        //}
        //return Result<ProductModel>.Successful(productModel);
        //    });

        #endregion

    }
}
