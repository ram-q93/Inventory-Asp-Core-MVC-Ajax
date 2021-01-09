using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Common
{
    public interface ISeedData
    {
        Task<Result> LoadSampleData();
    }

    public class SeedData : ISeedData
    {
        private readonly IRepository _repository;
        private readonly InventoryDbContext _dbcontext;
        private readonly ISerializer _serializer;
        private readonly ILogger _logger;

        public SeedData() { }

        public SeedData(IRepository repository, InventoryDbContext dbcontext,
            ISerializer serializer, ILogger logger)
        {
            _repository = repository;
            _dbcontext = dbcontext;
            _serializer = serializer;
            _logger = logger;
        }

        #region LoadSampleData
        public Task<Result> LoadSampleData() =>
            Result.TryAsync(async () =>
            {
                var result = await _repository.CountAllAsync<Storage>();
                if (result.Data == 0)
                {
                    //-----------Request for image------------//
                    //var tasks = new ConcurrentBag<Task<Result<Image>>>();
                    //for (int i = 0; i < 30; i++)
                    //{
                    //    tasks.Add(new InventoryHttpClient(_serializer, _logger).SendHttpRequestToGetImageByteArray());
                    //    Thread.Sleep(50);
                    //}
                    //await Task.WhenAll(tasks);

                    //string path = Path.Combine(Environment.CurrentDirectory, "wwwroot/seeddata/image.json");
                    //var imageJsonArray = _serializer.SerializeToJson(tasks
                    //    .Where(t => t.Result.Success).Select(t => t.Result.Data).ToList())
                    //    .Replace("}", "}" + Environment.NewLine);
                    //await File.AppendAllTextAsync(path, imageJsonArray);
                    //---------- Request for image------------//

                    string supplierFile = File.ReadAllText(Path.Combine(Environment.CurrentDirectory,
                        "wwwroot/seeddata/supplier.json"));
                    string productFile = File.ReadAllText(Path.Combine(Environment.CurrentDirectory,
                        "wwwroot/seeddata/product.json"));
                    string storageFile = File.ReadAllText(Path.Combine(Environment.CurrentDirectory,
                        "wwwroot/seeddata/storage.json"));
                    string categoryFile = File.ReadAllText(Path.Combine(Environment.CurrentDirectory,
                        "wwwroot/seeddata/category.json"));
                    string imageFile = File.ReadAllText(Path.Combine(Environment.CurrentDirectory,
                        "wwwroot/seeddata/image.json"));

                    var suppliers = _serializer.DeserializeFromJson<IList<Supplier>>(supplierFile).ToList();
                    var products = _serializer.DeserializeFromJson<IList<Product>>(productFile).ToList();
                    var storages = _serializer.DeserializeFromJson<IList<Storage>>(storageFile).ToList();
                    var categories = _serializer.DeserializeFromJson<IList<Category>>(categoryFile).ToList();
                    var images = _serializer.DeserializeFromJson<IList<Image>>(imageFile).ToList();

                    //suppliers.ForEach(s => _repository.Add(s));
                    //await _repository.CommitAsync();
                    //_logger.Info($"suppliers persisted ");

                    //storages.ForEach(s => _repository.Add(s));
                    //await _repository.CommitAsync();
                    //_logger.Info($"storages persisted ");

                    //images.ForEach(s => _repository.Add(s));
                    //await _repository.CommitAsync();
                    //_logger.Info($"images persisted ");

                    //categories.ForEach(s => _repository.Add(s));
                    //await _repository.CommitAsync();
                    //_logger.Info($"categories persisted ");


                    var storageIds = _dbcontext.Storages.Select(s => s.Id).ToList();
                    var supplierIds = _dbcontext.Suppliers.Select(s => s.Id).ToList();
                    var imageIds = _dbcontext.Images.Select(i => i.Id).ToList();
                    var categoryIds = _dbcontext.Categories.Select(c => c.Id).ToList();

                    products.ForEach(p =>
                    {
                        p.ImageId = imageIds[new Random().Next(0, imageIds.Count())];
                        p.CategoryId = categoryIds[new Random().Next(0, categoryIds.Count())];
                        p.SupplierId = supplierIds[new Random().Next(0, supplierIds.Count())];
                        p.StorageId = storageIds[new Random().Next(0, storageIds.Count())];
                        _repository.Add(p);
                    });
                    await _repository.CommitAsync();
                    _logger.Info($"products persisted ");
                }
                return Result.Successful();
            });

        #endregion
    }
}
