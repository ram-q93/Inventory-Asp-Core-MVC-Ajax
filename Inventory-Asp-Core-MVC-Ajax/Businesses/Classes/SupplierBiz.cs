using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using PagedList.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class SupplierBiz : ISupplierBiz
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public SupplierBiz(
            IRepository repository,
            IMapper mapper,
            ILogger logger)

        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        #region GetSupplierPagedListFilteredBySearchQuery

        public Task<Result<SupplierFilterModel>> GetSupplierPagedListFilteredBySearchQuery(int? page, string searchQuery) =>
            Result<SupplierFilterModel>.TryAsync(async () =>
            {
                var pagingModel = new PagingModel()
                {
                    PageNumber = (page == null || page <= 0 ? 1 : page.Value) - 1,
                    PageSize = 5,
                    Sort = "UpdatedDate",
                    SortDirection = SortDirection.DESC
                };
                var resultList = await repository.ListAsNoTrackingAsync<Supplier>(s => searchQuery == null ||
                    (s.Name != null && s.Name.Contains(searchQuery)) ||
                    (s.Address != null && s.Address.Contains(searchQuery)),
                    pagingModel, "UpdatedDate");

                if (!resultList.Success)
                {
                    return Result<SupplierFilterModel>.Failed(Error.WithCode(ErrorCodes.SuppliersNotFound));
                }
                return Result<SupplierFilterModel>.Successful(new SupplierFilterModel()
                {
                    SupplierPagedList = new StaticPagedList<SupplierModel>(
                       resultList.Items.Select(s => mapper.Map<Supplier, SupplierModel>(s)),
                       resultList.PageNumber + 1,
                       resultList.PageSize,
                       (int)resultList.TotalCount),
                    SearchQuery = searchQuery
                });
            });


        #endregion

        #region GetById

        public Task<Result<SupplierModel>> GetById(int id) =>
            Result<SupplierModel>.TryAsync(async () =>
            {
                var result = await repository.FirstOrDefaultAsNoTrackingAsync<Supplier>(p => p.Id == id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result<SupplierModel>.Failed(Error.WithCode(ErrorCodes.SupplierNotFoundById));
                }
                return Result<SupplierModel>.Successful(mapper.Map<Supplier, SupplierModel>(result.Data));
            });

        #endregion

        #region Add

        public Task<Result> Add(SupplierModel model) =>
            Result.TryAsync(async () =>
            {
                var store = mapper.Map<SupplierModel, Supplier>(model);
                store.CreatedDate = DateTime.Now;
                store.UpdatedDate = DateTime.Now;
                repository.Add(store);
                await repository.CommitAsync();
                logger.Info($"Supplier Added:{model}");
                return Result.Successful();
            });

        #endregion

        #region Edit
        public Task<Result> Edit(SupplierModel model) =>
            Result.TryAsync(async () =>
            {
                var result = await repository.FirstOrDefaultAsNoTrackingAsync<Supplier>(p => p.Id == model.Id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.SupplierNotFoundById));
                }
                var store = mapper.Map<SupplierModel, Supplier>(model);
                store.UpdatedDate = DateTime.Now;
                repository.Update(store);
                await repository.CommitAsync();
                logger.Info($"Supplier Edited:{model}");
                return Result.Successful();
            });

        #endregion

        #region Delete

        public Task<Result> Delete(int id) =>
            Result.TryAsync(async () =>
            {
                var result = await repository.FirstOrDefaultAsNoTrackingAsync<Supplier>(p => p.Id == id,
                    includes: p => p.Products.Select(p => p.Image));
                if (!result.Success || result?.Data == null)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.SupplierNotFoundById));
                }
                result.Data.Products.Clear();
                repository.Remove(result.Data);
                await repository.CommitAsync();
                logger.Warn($"Supplier Deleted: Id={result.Data.Id} Name={result.Data.Name}");
                return Result.Successful();
            });

        #endregion

        #region Details

        public Task<Result<SupplierModel>> Details(int id) =>
            Result<SupplierModel>.TryAsync(async () =>
            {
                var result = await repository.FirstOrDefaultAsNoTrackingAsync<Supplier>(p => p.Id == id);
                if (!result.Success)
                {
                    return Result<SupplierModel>.Failed(Error.WithCode(ErrorCodes.SupplierDetailsNotFoundById));
                }
                var supplierModel = mapper.Map<Supplier, SupplierModel>(result.Data);
                return Result<SupplierModel>.Successful(supplierModel);
            });

        #endregion
    }
}
