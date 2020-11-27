using AutoMapper;
using Helper.Library.Enums;
using Helper.Library.Models;
using Helper.Library.Services;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using InventoryProject.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<ResultList<StorageModel>> List(PagingModel pagingModel)
        {
            await LoadSampleData();
            PagingModel p = null;

            if (pagingModel.Sort == null)
                p = new PagingModel<Storage>(s => s.UpdatedDate, SortDirection.DESC);
            else
                p = new PagingModel<Storage>(pagingModel, s => s.UpdatedDate);
            var resultList = await repository.ListAsNoTrackingAsync<Storage>(p);
            pagingModel.PageNumber = 0;
            pagingModel.PageSize = 10;
            pagingModel.Sort = "UpdatedDate";
            pagingModel.SortDirection = SortDirection.DESC;
            var resultList2 = await repository.ListAsNoTrackingAsync<Storage>(pagingModel);
            // var r = await Listttttt(repository.GetCurrentContext(), p);
            if (!resultList.Success)
            {
                return ResultList<StorageModel>.Failed(Error.WithCode(ErrorCodes.StoragesNotFound));
            }
            return new ResultList<StorageModel>()
            {
                Items = resultList.Items.Select(store => mapper.Map<Storage, StorageModel>(store)),
                PageNumber = resultList.PageNumber,
                PageSize = resultList.PageSize,
                TotalCount = resultList.TotalCount,
                Success = true
            };
        }

        public async Task<ResultList<TEntity>> Listttttt<TEntity>(DbContext db,
         PagingModel<TEntity> model)
         where TEntity : class
        {
            List<TEntity> result = await (Task<List<TEntity>>)EntityFrameworkQueryableExtensions
                .ToListAsync<TEntity>(((IQueryable<TEntity>)((DbContext)(object)db)
                .Set<TEntity>()).Sort<TEntity>(model.SortBy, model.SortDirection)
                .Skip<TEntity>(model.PageNumber * model.PageSize).Take<TEntity>(model.PageSize), new CancellationToken());
            IEnumerable<TEntity> items = (IEnumerable<TEntity>)result;
            int num = await EntityFrameworkQueryableExtensions.CountAsync<TEntity>(((IQueryable<TEntity>)((DbContext)(object)db).Set<TEntity>()), new CancellationToken());
            return ResultList<TEntity>.Successful(items, (long)num, model.PageNumber, model.PageSize);
        }



        #endregion

        #region GetById

        public async Task<Result<StorageModel>> GetById(int id)
        {
            var result = await repository.FirstOrDefaultAsNoTrackingAsync<Storage>(p => p.Id == id);
            if (result?.Success != true || result?.Data == null)
            {
                return Result<StorageModel>.Failed(Error.WithCode(ErrorCodes.StorageNotFoundById));
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
                return Result.Failed(Error.WithCode(ErrorCodes.StorageNotFoundById));
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
                return Result.Failed(Error.WithCode(ErrorCodes.StorageNotFoundById));
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
                return Result<StorageModel>.Failed(Error.WithCode(ErrorCodes.StorageProductsNotFound));
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
                logger.Info($"db persist duration : {watch.ElapsedMilliseconds}");
            }
        }

        #endregion
    }

    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Sort<TEntity>(
          this IQueryable<TEntity> query,
          Expression<Func<TEntity, object>> sortBy,
          SortDirection direction)
        {
            if (sortBy == null)
                return query;
            switch (direction)
            {
                case SortDirection.ASC:
                    return (IQueryable<TEntity>)query.OrderBy<TEntity, object>(sortBy);
                case SortDirection.DESC:
                    return (IQueryable<TEntity>)query.OrderByDescending<TEntity, object>(sortBy);
                default:
                    return (IQueryable<TEntity>)query.OrderBy<TEntity, object>(sortBy);
            }
        }

        public static IQueryable<TEntity> Sort<TEntity>(
          this IQueryable<TEntity> query,
          params SortModel<TEntity>[] sorts)
        {
            ((IEnumerable<SortModel<TEntity>>)sorts).ToList<SortModel<TEntity>>().ForEach((Action<SortModel<TEntity>>)(sort => query.Sort<TEntity>(sort.SortBy, sort.SortDirection)));
            return query;
        }
    }
}
