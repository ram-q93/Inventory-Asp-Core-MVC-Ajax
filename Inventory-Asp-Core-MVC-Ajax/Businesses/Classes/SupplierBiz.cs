using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class SupplierBiz : ISupplierBiz
    {
        private readonly IRepository _repository;
        private readonly IInventoryDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SupplierBiz(IRepository repository, IInventoryDbContext dbContext,
            IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _dbContext = dbContext;
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

                var resultList = await _repository.SortedPageListAsNoTrackingAsync<Supplier>(s =>
                    searchBy == null ||
                    (s.CompanyName != null && s.CompanyName.Contains(searchBy)) ||
                    (s.ContactName != null && s.ContactName.Contains(searchBy)) ||
                    (s.EmergencyMobile != null && s.EmergencyMobile.Contains(searchBy)) ||
                    (s.Phone != null && s.Phone.Contains(searchBy)) ||
                    (s.Address != null && s.Address.Contains(searchBy)) ||
                    (s.City != null && s.City.Contains(searchBy)) ||
                    (s.PostalCode != null && s.PostalCode.Contains(searchBy)),
                    pagingModel);

                if (!resultList.Success)
                {
                    return Result<object>.Failed(
                        Error.WithData(ErrorCodes.SuppliersNotFound, new[] { "Some thing went wrong!" }));
                }

                var totalFilteredCount = resultList.TotalCount;
                var totalCount = (await _repository.CountAllAsync<Supplier>()).Data;
                var supplierModels = _mapper.Map<IEnumerable<SupplierModel>>(resultList.Items);

                return Result<object>.Successful(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalFilteredCount,
                    data = supplierModels
                });
            });

        #endregion

        #region GetById

        public Task<Result<SupplierModel>> GetById(int id) =>
            Result<SupplierModel>.TryAsync(async () =>
            {
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Supplier>(p => p.Id == id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result<SupplierModel>.Failed(
                        Error.WithData(ErrorCodes.SupplierNotFoundById, new[] { "Supplier not found!" }));
                }
                return Result<SupplierModel>.Successful(_mapper.Map<Supplier, SupplierModel>(result.Data));
            });

        #endregion

        #region Add

        public Task<Result> Add(SupplierModel model) =>
            Result.TryAsync(async () =>
            {
                if ((await IsNameInUse(model.CompanyName)).Data)
                {
                    return Result.Failed(
                       Error.WithData(ErrorCodes.SupplierNameAlreadyInUse,
                       new[] { "Supplier name already in use!" }));
                }

                var supplier = _mapper.Map<SupplierModel, Supplier>(model);
                _repository.Add(supplier);
                await _repository.CommitAsync();

                return Result.Successful();
            });

        #endregion

        #region Edit

        public Task<Result> Edit(SupplierModel model) =>
            Result.TryAsync(async () =>
            {
                if ((await IsNameInUse(model.CompanyName, model.Id)).Data)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.SupplierNameAlreadyInUse,
                      new[] { "Supplier name already in use!" }));
                }
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Supplier>(p => p.Id == model.Id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.SupplierNotFoundById,
                        new[] { "Supplier not found!" }));
                }

                var supplier = _mapper.Map<SupplierModel, Supplier>(model);
                _repository.Update(supplier);
                await _repository.CommitAsync();

                return Result.Successful();
            });

        #endregion

        #region Delete

        public Task<Result> Delete(int id) =>
            Result.TryAsync(async () =>
            {
                var supplierResult = await _repository.FirstOrDefaultAsNoTrackingAsync<Supplier>(p => p.Id == id);
                if (!supplierResult.Success || supplierResult?.Data == null)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.SupplierNotFoundById,
                        new[] { "Supplier not found!" }));
                }
                _repository.Remove(supplierResult.Data);
                await _repository.CommitAsync();
                _logger.Warn($"Supplier Deleted: {supplierResult.Data.CompanyName}");
                return Result.Successful();
            });

        #endregion

        #region IsNameInUse

        public Task<Result<bool>> IsNameInUse(string name, int? id = null) =>
            Result<bool>.TryAsync(async () =>
            {
                var result = await _repository.ExistsAsync<Supplier>(s => s.CompanyName == name &&
                (id == null || s.Id != id));// check id for edit: 
                return Result<bool>.Successful(result.Data);
            });

        #endregion

        #region ListName
        public Result<object> ListName() =>
            Result<object>.Try(() =>
            {
                var result = _dbContext.Suppliers.Where(s => s.Enabled)
                   .Select(s => new { s.Id, s.CompanyName })
                   .OrderBy(s => s.CompanyName).ToList();
                return Result<object>.Successful(result);
            });
        #endregion
    }
}
