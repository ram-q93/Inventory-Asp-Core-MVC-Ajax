using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using InventoryProject.Business.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class StorageBiz : IStorageBiz
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ISerializer _serializer;
        private readonly ILogger _logger;


        public StorageBiz(
            IRepository repository,
            IMapper mapper,
            ISerializer serializer,
            ILogger logger)

        {
            _repository = repository;
            _mapper = mapper;
            _serializer = serializer;
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

                var resultList = await _repository.SortedPageListAsNoTrackingAsync<Storage>(s =>
                                        searchBy == null ||
                                        (s.Name != null && s.Name.ToUpper().Contains(searchBy.ToUpper())) ||
                                        (s.Address != null && s.Address.ToUpper().Contains(searchBy.ToUpper())) ||
                                        (s.City != null && s.City.ToUpper().Contains(searchBy.ToUpper())) ||
                                        (s.Phone != null && s.Phone.ToUpper().Contains(searchBy.ToUpper())),
                                        pagingModel);
                
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
                    return Result<StorageModel>.Failed(Error.WithCode(ErrorCodes.StorageNotFoundById));
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
                    return Result.Failed(Error.WithCode(ErrorCodes.StorageNameAlreadyInUse));
                }
                var store = _mapper.Map<StorageModel, Storage>(model);
                _repository.Add(store);
                await _repository.CommitAsync();
                return Result.Successful();
            });

        #endregion

        #region Edit

        public Task<Result> Edit(StorageModel model) =>
            Result.TryAsync(async () =>
            {
                if ((await IsNameInUse(model.Name, model.Id)).Data)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.StorageNameAlreadyInUse));
                }
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Storage>(p => p.Id == model.Id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.StorageNotFoundById));
                }
                var storage = _mapper.Map<StorageModel, Storage>(model);
                _repository.Update(storage);
                await _repository.CommitAsync();
                return Result.Successful();
            });

        #endregion

        #region Delete

        public Task<Result> Delete(int id) =>
            Result.TryAsync(async () =>
            {
                var storeResult = await _repository.FirstOrDefaultAsNoTrackingAsync<Storage>(p => p.Id == id,
                    includes: p => p.Products.Select(p => p.Image));
                if (!storeResult.Success || storeResult?.Data == null)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.StorageNotFoundById));
                }
                storeResult.Data.Products.Clear();
                _repository.Remove(storeResult.Data);
                await _repository.CommitAsync();
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

        #region LoadSampleData
        private Task<Result> LoadSampleData() =>
            Result.TryAsync(async () =>
            {




                var storageRresult = await _repository.ListAsNoTrackingAsync<Storage>();
                //for (int i = 0; i < 38; i++)
                //{
                //    for (int j = 0; j < 100; j++)
                //    {
                //        _repository.Add(new Storage()
                //        {
                //            Address = storageRresult.Data[i].Address,
                //            City = "Tehran" + j,
                //            Enabled = true,
                //            CreatedBy = "ramin",
                //            LastModified = DateTime.Now,
                //            LastModifiedBy = "amir",
                //            Name = storageRresult.Data[i].Name + j,
                //            Phone = storageRresult.Data[i].Phone
                //        });
                //    }
                //    await _repository.CommitAsync();
                //    Console.WriteLine("added");
                //}

                if (storageRresult?.Data?.Count == 0)
                {
                    //-----------Request for image------------//
                    var tasks = new ConcurrentBag<Task<Result<byte[]>>>();
                    for (int i = 0; i < 40; i++)
                    {
                        tasks.Add(new InventoryHttpClient(_serializer, _logger).SendHttpRequestToGetImageByteArray());
                        Thread.Sleep(50);
                    }
                    await Task.WhenAll(tasks);
                    var imageByteArrList = tasks.Where(t => t.Result.Success).Select(t => t.Result.Data).ToList();
                    imageByteArrList = Enumerable.Repeat(imageByteArrList, 700).SelectMany(arr => arr).ToList();
                    //---------- Request for image------------//

                    string supplierFile = System.IO.File.ReadAllText("Supplier.json");
                    var suppliers = _serializer.DeserializeFromJson<IList<Supplier>>(supplierFile).ToList();
                    string file = System.IO.File.ReadAllText("generated.json");
                    var storages = _serializer.DeserializeFromJson<IList<Storage>>(file).ToList();
                    var index = 1;
                    storages.ForEach(s =>
                    {
                        s.Products.ToList().ForEach(p =>
                        {
                            p.Supplier = suppliers[new Random().Next(1, 12)];
                            imageByteArrList?.RemoveAt(1);
                            p.Image = new Image()
                            {
                                Title = $"{p.Name}-{new Random().Next(30, 100000)}.jpg",
                                Data = imageByteArrList[index++],
                                Caption = $"This is caption...{new Random().Next(30, 100000)}"
                            };

                        });
                    });
                    storages.ForEach(s => _repository.Add(s));
                    var watch = Stopwatch.StartNew();
                    watch.Start();
                    await _repository.CommitAsync();
                    watch.Stop();
                    _logger.Info($"db persist duration : {watch.ElapsedMilliseconds}");
                }
                return Result.Successful();
            });

        #endregion
    }
}
