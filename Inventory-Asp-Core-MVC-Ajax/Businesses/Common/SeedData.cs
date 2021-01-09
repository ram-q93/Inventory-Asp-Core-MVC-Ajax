using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Common
{
    public class SeedData
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ISerializer _serializer;
        private readonly ILogger _logger;


        public SeedData(
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
                            //p.Image = new Image()
                            //{
                            //    Title = $"{p.Name}-{new Random().Next(30, 100000)}.jpg",
                            //    Data = imageByteArrList[index++],
                            //    Caption = $"This is caption...{new Random().Next(30, 100000)}"
                            //};

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
