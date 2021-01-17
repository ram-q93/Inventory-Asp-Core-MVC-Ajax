using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services.Interfaces;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Core;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class CategoryBiz : ICategoryBiz
    {
        private readonly IRepository _repository;
        private readonly IInventoryDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CategoryBiz(IRepository repository, IInventoryDbContext dbContext,
            ILogger logger, IMapper mapper)
        {
            _repository = repository;
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
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

                var resultList = await _repository.SortedPageListAsNoTrackingAsync<Category>(p =>
                           searchBy == null || (p.Name != null && p.Name.Contains(searchBy)), pagingModel);

                if (!resultList.Success)
                {
                    return Result<object>.Failed(Error.WithCode(ErrorCodes.CategoriesNotFound), "Some thing went wrong!");
                }

                var totalCount = (await _repository.CountAllAsync<Category>()).Data;
                var totalFilteredCount = resultList.TotalCount;
                var categoryModels = _mapper.Map<IEnumerable<CategoryModel>>(resultList.Items);

                return Result<object>.Successful(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalFilteredCount,
                    data = categoryModels
                });
            });

        #endregion

        #region GetById

        public Task<Result<CategoryModel>> GetById(int id) =>
            Result<CategoryModel>.TryAsync(async () =>
            {
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Category>(p => p.Id == id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result<CategoryModel>.Failed(Error.WithCode(ErrorCodes.CategoryNotFoundById), "Category not found!");
                }

                var productModel = _mapper.Map<Category, CategoryModel>(result.Data);

                return Result<CategoryModel>.Successful(productModel);
            });

        #endregion

        #region Add

        public Task<Result> Add(CategoryModel model) =>
            Result.TryAsync(async () =>
            {
                if ((await IsNameInUse(model.Name)).Data)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.CategoryNameAlreadyInUse), "Category name already in use!");
                }

                var newCategory = _mapper.Map<CategoryModel, Category>(model);

                _repository.Add(newCategory);
                await _repository.CommitAsync();
                return Result.Successful();
            });

        #endregion

        #region Edit

        public Task<Result> Edit(CategoryModel model) =>
            Result.TryAsync(async () =>
            {
                if ((await IsNameInUse(model.Name, model.Id)).Data)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.CategoryNameAlreadyInUse), "Category name already in use!");
                }

                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Category>(p => p.Id == model.Id);
                if (result?.Success != true || result?.Data == null)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.CategoryNotFoundById), "Category not found!");
                }

                var category = result.Data;
                category.Name = model.Name;
                category.Description = model.Description;

                _repository.Update(category);
                await _repository.CommitAsync();
                return Result.Successful();
            });

        #endregion

        #region Delete

        public Task<Result> Delete(int id) =>
            Result.TryAsync(async () =>
            {
                var result = await _repository.FirstOrDefaultAsNoTrackingAsync<Category>(p => p.Id == id);
                if (!result.Success || result?.Data == null)
                {
                    return Result.Failed(Error.WithCode(ErrorCodes.CategoryNotFoundById), "Category not found!");
                }

                _repository.Remove(result.Data);
                await _repository.CommitAsync();
                _logger.Warn($"Category Deleted  { result.Data.Name}");
                return Result.Successful();
            });

        #endregion

        #region IsNameInUse

        public Task<Result<bool>> IsNameInUse(string name, int? id = null) =>
            Result<bool>.TryAsync(async () =>
            {
                var result = await _repository.ExistsAsync<Category>(s => s.Name == name &&
                (id == null || s.Id != id));
                return Result<bool>.Successful(result.Data);
            });

        #endregion

        #region ListName
        public Result<object> ListName() =>
            Result<object>.Try(() =>
           {
               var result = _dbContext.Categories.Select(c => new { c.Id, c.Name })
                    .OrderBy(c => c.Name).ToList();
               return Result<object>.Successful(result);
           });
        #endregion


    }
}
