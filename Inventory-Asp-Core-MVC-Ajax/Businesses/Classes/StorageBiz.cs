using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using InventoryProject.Business.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class StorageBiz : IStorageBiz
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly InventoryDbContext db;

        public StorageBiz(IRepository repository, IMapper mapper, ILogger logger, InventoryDbContext db)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            this.db = db;
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

                var resultList = await _repository.SortedPageListAsNoTrackingAsync<Storage>(s =>
                                        searchBy == null ||
                                        (s.Name != null && s.Name.ToUpper().Contains(searchBy.ToUpper())) ||
                                        (s.Address != null && s.Address.ToUpper().Contains(searchBy.ToUpper())) ||
                                        (s.City != null && s.City.ToUpper().Contains(searchBy.ToUpper())) ||
                                        (s.Phone != null && s.Phone.ToUpper().Contains(searchBy.ToUpper())),
                                        pagingModel);
                
                if (!resultList.Success)
                {
                    return Result<object>.Failed(
                        Error.WithData(ErrorCodes.StoragesNotFound, new[] { "Some thing went wrong!" }));
                }

                var totalFilteredCount = resultList.TotalCount;
                var totalCount = (await _repository.CountAllAsync<Storage>()).Data;
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

        public Task<Result<StorageModel>> GetById(int id) =>
            Result<StorageModel>.TryAsync(async () =>
            {
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Storage>(p => p.Id == id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result<StorageModel>.Failed(
                        Error.WithData(ErrorCodes.StorageNotFoundById, new[] { "Storage not found!" }));
                }
                return Result<StorageModel>.Successful(_mapper.Map<Storage, StorageModel>(result.Data));
            });

        #endregion

        #region Add

        public Task<Result> Add(StorageModel model) =>
            Result.TryAsync(async () =>
            {
                if ((await IsNameInUse(model.Name)).Data)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.StorageNameAlreadyInUse, 
                        new[] { "Storage name already in use!" }));
                }
                var storage = _mapper.Map<StorageModel, Storage>(model);
                _repository.Add(storage);
                await _repository.CommitAsync();
                _logger.Info($"Storage Added:{model}");
                return Result.Successful();
            });

        #endregion

        #region Edit

        public Task<Result> Edit(StorageModel model) =>
            Result.TryAsync(async () =>
            {
                if ((await IsNameInUse(model.Name, model.Id)).Data)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.StorageNameAlreadyInUse, 
                        new[] { "Storage name already in use!" }));
                }
                var result = await _repository.FirstOrDefaultAsync<Storage>(p => p.Id == model.Id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.StorageNotFoundById, 
                        new[] { "Storage not found!" }));
                }
                result.Data.Name = model.Name;
                result.Data.Phone = model.Phone;
                result.Data.Enabled = model.Enabled;
                result.Data.City = model.City;
                result.Data.Address = model.Address;
                await _repository.CommitAsync();
                _logger.Info($"Storage Edited:{model}");
                return Result.Successful();
            });

        #endregion

        #region Delete

        public Task<Result> Delete(int id) =>
            Result.TryAsync(async () =>
            {
                var storageResult = await _repository.FirstOrDefaultAsNoTrackingAsync<Storage>(p => p.Id == id);
                if (!storageResult.Success || storageResult?.Data == null)
                {
                    return Result.Failed(Error.WithData(ErrorCodes.StorageNotFoundById,
                        new[] { "Storage not found!" }));
                }
                _repository.Remove(storageResult.Data);
                await _repository.CommitAsync();
                _logger.Warn($"Storage Deleted:{storageResult.Data.Name}");
                return Result.Successful();
            });

        #endregion

        #region IsNameInUse

        public Task<Result<bool>> IsNameInUse(string name, int? id = null) =>
            Result<bool>.TryAsync(async () =>
            {
                var result = await _repository.ExistsAsync<Storage>(s => s.Name == name &&
                (id == null || s.Id != id));// check id for edit: 
                return Result<bool>.Successful(result.Data);
            });

        #endregion

    }
}
