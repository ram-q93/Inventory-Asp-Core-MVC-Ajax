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

        public Task<ResultList<StorageModel>> List(PagingModel pagingModel, string searchQuery) =>
            ResultList<StorageModel>.TryAsync(async () =>
            {
                await LoadSampleData();
                var resultList = await _repository.ListAsNoTrackingAsync<Storage>(s => searchQuery == null ||
                    (s.Name != null && s.Name.Contains(searchQuery)) ||
                    (s.Phone != null && s.Phone.Contains(searchQuery)) ||
                    (s.Address != null && s.Address.Contains(searchQuery)),
                    pagingModel, pagingModel.Sort);

                //var r = await repository.ListAsNoTrackingAsync<Storage>(
                //    new PagingModel<Storage>(pagingModel, s => s.UpdatedDate));

                if (!resultList.Success)
                {
                    return ResultList<StorageModel>.Failed(Error.WithCode(ErrorCodes.StoragesNotFound));
                }
                return new ResultList<StorageModel>()
                {
                    Success = true,
                    Items = resultList.Items.Select(store => _mapper.Map<Storage, StorageModel>(store)).ToList(),
                    PageNumber = resultList.PageNumber,
                    PageSize = resultList.PageSize,
                    TotalCount = resultList.TotalCount
                };
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
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Storage>(s => s.Name == name &&
                (id == null || s.Id != id));// check id for edit: 
                return Result<bool>.Successful(result.Data != null);
            });

        #endregion

        #region LoadSampleData
        private Task<Result> LoadSampleData() =>
            Result.TryAsync(async () =>
            {
                var storageRresult = await _repository.ListAsNoTrackingAsync<Storage>();
                if (storageRresult?.Data?.Count == 0)
                {
                    //-----------Request for image------------//
                    var tasks = new ConcurrentBag<Task<Result<byte[]>>>();
                    for (int i = 0; i < 10; i++)
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

        public static IQueryable<TEntity> Sort<TEntity>(this IQueryable<TEntity> query, PagingModel model, string propertyName)
        {
            if (propertyName == null)
                return query;
            // LAMBDA: x => x.[PropertyName]
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            // REFLECTION: source.OrderBy(x => x.Property)
            MethodInfo orderByMethod = null;
            switch (model.SortDirection)
            {
                case SortDirection.ASC:
                    orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
                    break;
                case SortDirection.DESC:
                    orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);
                    break;
                default:
                    orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
                    break;
            }
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TEntity), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { query, lambda });
            return (IQueryable<TEntity>)result;
        }

        // public async Task<ResultList<TEntity>> ListAsNoTrackingAsync<TEntity>(DbContext context,
        //PagingModel model, string Sort)
        //where TEntity : class
        // {
        //     List<TEntity> result = await (Task<List<TEntity>>)EntityFrameworkQueryableExtensions.ToListAsync<TEntity>(((IQueryable<TEntity>)EntityFrameworkQueryableExtensions.AsNoTracking<TEntity>(((DbContext)(object)context).Set<TEntity>())).Sort<TEntity>(model, Sort).Skip<TEntity>(model.PageNumber * model.PageSize).Take<TEntity>(model.PageSize), new CancellationToken());
        //     IEnumerable<TEntity> items = (IEnumerable<TEntity>)result;
        //     int num = await EntityFrameworkQueryableExtensions.CountAsync<TEntity>(((DbContext)(object)context).Set<TEntity>(), new CancellationToken());
        //     return ResultList<TEntity>.Successful(items, (long)num, model.PageNumber, model.PageSize);
        // }


        // public async Task<ResultList<TEntity>> Listttttt<TEntity>(DbContext db, PagingModel<TEntity> model)
        //  where TEntity : class
        // {
        //     List<TEntity> result = await (Task<List<TEntity>>)EntityFrameworkQueryableExtensions
        //         .ToListAsync<TEntity>(((IQueryable<TEntity>)((DbContext)(object)db)
        //         .Set<TEntity>()).Sort<TEntity>(model.SortBy, model.SortDirection)
        //         .Skip<TEntity>(model.PageNumber * model.PageSize).Take<TEntity>(model.PageSize), new CancellationToken());
        //     IEnumerable<TEntity> items = (IEnumerable<TEntity>)result;
        //     int num = await EntityFrameworkQueryableExtensions.CountAsync<TEntity>(((IQueryable<TEntity>)((DbContext)(object)db).Set<TEntity>()), new CancellationToken());
        //     return ResultList<TEntity>.Successful(items, (long)num, model.PageNumber, model.PageSize);
        // }

    }
}
