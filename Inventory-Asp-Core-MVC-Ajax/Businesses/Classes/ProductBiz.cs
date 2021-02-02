using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Extensions;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services.Interfaces;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Core;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class ProductBiz : IProductBiz
    {
        private readonly IInventoryDbContext _context;
        private readonly IRepository _repository;
        private readonly IImageBiz _imageBiz;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProductBiz(IInventoryDbContext context, IRepository repository,
            IImageBiz imageBiz, IMapper mapper, ILogger logger)
        {
            _context = context;
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


                IQueryable<ProductModel> query = _context.Products.AsNoTracking()
                        .Where(p =>
                            p.Storage != null && p.Storage.Enabled &&
                            p.Supplier != null && p.Supplier.Enabled &&
                            (searchBy == null ||
                            (p.Name != null && p.Name.Contains(searchBy)) ||
                            (p.Code != null && p.Code.Contains(searchBy)) ||
                            (p.Description != null && p.Description.Contains(searchBy)) ||
                            (p.Storage != null && p.Storage.Name.Contains(searchBy)) ||
                            (p.Supplier != null && p.Supplier.CompanyName.Contains(searchBy))))
                       .Include(p => p.Category)
                       .Include(p => p.Storage)
                       .Include(p => p.Supplier)
                       .Select(p => new ProductModel()
                       {
                           Id = p.Id,
                           Name = p.Name,
                           Code = p.Code,
                           Quantity = p.Quantity,
                           UnitePrice = p.UnitePrice,
                           Description = p.Description,
                           Enabled = p.Enabled,
                           CategoryName = p.Category == null ? null : p.Category.Name,
                           StorageName = p.Storage == null ? null : p.Storage.Name,
                           SupplierCompanyName = p.Supplier == null ? null : p.Supplier.CompanyName
                       })
                       .SortByStringField(pagingModel);

                var totalFilteredCount = await query.CountAsync();

                List<ProductModel> resultProductModels = await query
                        .Skip(pagingModel.PageNumber * pagingModel.PageSize)
                        .Take(pagingModel.PageSize).ToListAsync();

                var totalCount = (await _repository.CountAllAsync<Product>()).Data;

                return Result<object>.Successful(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalFilteredCount,
                    data = resultProductModels
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
                    return Result<ProductModel>.Failed(Error.WithCode(ErrorCodes.ProductNotFoundById), "Product not found!");
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
                    return Result.Failed(Error.WithCode(ErrorCodes.ProductNameAlreadyInUse), "Product name already in use!");
                }

                var newProduct = _mapper.Map<ProductModel, Product>(model);

                //------- image -------
                var imageResult = _imageBiz.CreateImage(model.ProductPicture);
                if (!imageResult.Success)
                {
                    return Result.Failed(imageResult.Error);
                }
                newProduct.Image = imageResult.Data;

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
                    return Result.Failed(Error.WithCode(ErrorCodes.ProductNameAlreadyInUse), "Product name already in use!");
                }

                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Product>(p => p.Id == model.Id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.ProductNotFoundById), "Product not found!");
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

                //----- image ------
                if (model.ProductPicture != null)
                {
                    var imageResult = _imageBiz.CreateImage(model.ProductPicture);
                    if (!imageResult.Success)
                    {
                        return Result.Failed(imageResult.Error);
                    }
                    product.Image = imageResult.Data;
                    product.Image.Id = Convert.ToInt32(model.ImageId);
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
                    return Result.Failed(Error.WithCode(ErrorCodes.ProductNotFoundById), "Product not found!");
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
                    return Result<ProductDetailsModel>.Failed(Error.WithCode(ErrorCodes.ProductNotFoundById), "Product not found!");
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



    }
}
