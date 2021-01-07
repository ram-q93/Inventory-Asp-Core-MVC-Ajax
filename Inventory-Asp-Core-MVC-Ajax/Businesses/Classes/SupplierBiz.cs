using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class SupplierBiz : ISupplierBiz
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SupplierBiz(IRepository repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        #region List

        public Task<Result<object>> List(DtParameters dtParameters) =>
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

                var resultList = await _repository.SortedPageListAsNoTrackingAsync<Supplier>(s => searchBy == null ||
                    (s.CompanyName != null && s.CompanyName.ToUpper().Contains(searchBy.ToUpper())) ||
                    (s.ContactName != null && s.ContactName.ToUpper().Contains(searchBy.ToUpper())) ||
                    (s.EmergencyMobile != null && s.EmergencyMobile.ToUpper().Contains(searchBy.ToUpper())) ||
                    (s.Phone != null && s.Phone.ToUpper().Contains(searchBy.ToUpper()) ||
                    (s.Address != null && s.Address.ToUpper().Contains(searchBy.ToUpper())) ||
                    (s.City != null && s.City.ToUpper().Contains(searchBy.ToUpper())) ||
                    (s.PostalCode != null && s.PostalCode.ToUpper().Contains(searchBy.ToUpper()))),
                    pagingModel);

                if (!resultList.Success)
                {
                    return Result<object>.Failed(
                        Error.WithData(ErrorCodes.SuppliersNotFound, new[] { "Some thing went wrong!" }));
                }

                var totalFilteredCount = resultList.TotalCount;
                var totalCount = (await _repository.CountAllAsync<Supplier>()).Data;
                return Result<object>.Successful(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalFilteredCount,
                    data = resultList.Items
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
                       Error.WithData(ErrorCodes.SupplierNameAlreadyInUse, new[] { "Supplier name already in use!" }));
                }
                var supplier = _mapper.Map<SupplierModel, Supplier>(model);
                _repository.Add(supplier);
                await _repository.CommitAsync();
                _logger.Info($"Supplier Added:{model}");
                return Result.Successful();
            });

        #endregion

        #region Edit

        public Task<Result> Edit(SupplierModel model) =>
            Result.TryAsync(async () =>
            {
                if ((await IsNameInUse(model.CompanyName, model.Id)).Data)
                {
                    return Result.Failed(
                      Error.WithData(ErrorCodes.SupplierNameAlreadyInUse, new[] { "Supplier name already in use!" }));
                }
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Supplier>(p => p.Id == model.Id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.SupplierNotFoundById, new[] { "Supplier not found!" }));
                }
                var supplier = _mapper.Map<SupplierModel, Supplier>(model);
                _repository.Update(supplier);
                await _repository.CommitAsync();
                _logger.Info($"Supplier Edited:{model}");
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
                    return Result.Failed(Error.WithData(ErrorCodes.SupplierNotFoundById, new[] { "Supplier not found!" }));
                }
                _repository.Remove(supplierResult.Data);
                await _repository.CommitAsync();
                _logger.Warn($"Supplier Deleted:{supplierResult.Data.CompanyName}");
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
    }
}
