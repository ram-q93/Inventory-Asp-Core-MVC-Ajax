using AutoMapper;
using Helper.Library.Models;
using Helper.Library.Services;
using Inventory_Asp_Core_MVC_Ajax.Api;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using InventoryProject.Business.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class StorageBiz : IStorageBiz
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly ISerializer serializer;
        private readonly ILogger logger;


        public StorageBiz(
            IRepository repository,
            IMapper mapper,
            ISerializer serializer,
            ILogger logger)

        {
            this.repository = repository;
            this.mapper = mapper;
            this.serializer = serializer;
            this.logger = logger;
        }


        #region List

        public async Task<Result<IList<StorageModel>>> List()
        {
            await LoadSampleData();
            var result = await repository.ListAsNoTrackingAsync<Storage>();
            if (!result.Success)
            {
                return Result<IList<StorageModel>>.Failed(Error.WithCode(ErrorCodes.StoresNotFound));
            }
            return Result<IList<StorageModel>>.Successful(
                result.Data.Select(store => mapper.Map<Storage, StorageModel>(store)).OrderByDescending(s => s.UpdatedDate).ToList());
        }

        #endregion

        #region GetById

        public async Task<Result<StorageModel>> GetById(int id)
        {
            var result = await repository.FirstOrDefaultAsNoTrackingAsync<Storage>(p => p.Id == id);
            if (result?.Success != true || result?.Data == null)
            {
                return Result<StorageModel>.Failed(Error.WithCode(ErrorCodes.StoreNotFoundById));
            }
            return Result<StorageModel>.Successful(mapper.Map<Storage, StorageModel>(result.Data));
        }

        #endregion

        #region Add

        public async Task<Result> Add(StorageModel model)
        {
            var store = mapper.Map<StorageModel, Storage>(model);
            store.CreatedDate = DateTime.Now;
            store.UpdatedDate = DateTime.Now;
            repository.Add(store);
            await repository.CommitAsync();
            return Result.Successful();
        }

        #endregion

        #region Edit
        public async Task<Result> Edit(StorageModel model)
        {
            var result = await repository.FirstOrDefaultAsNoTrackingAsync<Storage>(p => p.Id == model.Id);
            if (result?.Success != true || result?.Data == null)
            {
                return Result.Failed(Error.WithCode(ErrorCodes.StoreNotFoundById));
            }
            var store = mapper.Map<StorageModel, Storage>(model);
            store.UpdatedDate = DateTime.Now;
            repository.Update(store);
            await repository.CommitAsync();
            return Result.Successful();
        }

        #endregion

        #region Delete

        public async Task<Result> Delete(int id)
        {
            var storeResult = await repository.FirstOrDefaultAsNoTrackingAsync<Storage>(p => p.Id == id,
                includes: p => p.Products.Select(p => p.Images));
            if (!storeResult.Success || storeResult?.Data == null)
            {
                return Result.Failed(Error.WithCode(ErrorCodes.StoreNotFoundById));
            }
            storeResult.Data.Products.ToList().ForEach(p => p.Images.Clear());
            storeResult.Data.Products.Clear();
            repository.Remove(storeResult.Data);
            await repository.CommitAsync();
            return Result.Successful();
        }

        #endregion

        #region ListStorageAndProductsByStoreId

        public async Task<Result<StorageModel>> ListStorageAndProductsByStoreId(int storeId)
        {
            var productResults = await repository.FirstOrDefaultAsNoTrackingAsync<Storage>(s => s.Id == storeId,
                includes: s => s.Products);
            if (!productResults.Success)
            {
                return Result<StorageModel>.Failed(Error.WithCode(ErrorCodes.StoreProductsNotFound));
            }
            return Result<StorageModel>.Successful(mapper.Map<Storage, StorageModel>(productResults.Data));
        }

        #endregion

        #region LoadSampleData
        private async Task LoadSampleData()
        {
            var storageRresult = await repository.ListAsNoTrackingAsync<Storage>();
            if (storageRresult?.Data?.Count == 0)
            {
                //-----------Request for image------------//
                var tasks = new ConcurrentBag<Task<Result<byte[]>>>();
                for (int i = 0; i < 100; i++)
                {
                    tasks.Add(new InventoryHttpClient(serializer, logger).SendHttpRequestToGetImageByteArray());
                    Thread.Sleep(50);
                }
                await Task.WhenAll(tasks);
                var imageByteArrList = tasks.Where(t => t.Result.Success).Select(t => t.Result.Data).ToList();
                imageByteArrList = Enumerable.Repeat(imageByteArrList, 500).SelectMany(arr => arr).ToList();
                //---------- Request for image------------//

                string file = System.IO.File.ReadAllText("generated.json");
                var storages = serializer.DeserializeFromJson<IList<Storage>>(file).ToList();
                var index = 1;
                storages.ForEach(s =>
                {
                    s.CreatedDate = DateTime.Now;
                    s.UpdatedDate = DateTime.Now;
                    s.Products.ToList().ForEach(p =>
                    {
                        p.CreatedDate = DateTime.Now;
                        p.UpdatedDate = DateTime.Now;
                        imageByteArrList?.RemoveAt(1);
                        p.Images = Enumerable.Range(1, 8).Select(c =>
                        {
                            var image = new Image()
                            {
                                Title = $"{p.Name}-{new Random().Next(30, 100000)}{c}.jpg",
                                Data = imageByteArrList[index++]
                            };
                            return image;
                        }).ToList();
                    });
                });
                storages.ForEach(s => repository.Add(s));
                var watch = Stopwatch.StartNew();
                watch.Start();
                await repository.CommitAsync();
                watch.Stop();
                Console.WriteLine($"db persist duration : {watch.ElapsedMilliseconds}");
            }
        }

        #endregion
    }
}
