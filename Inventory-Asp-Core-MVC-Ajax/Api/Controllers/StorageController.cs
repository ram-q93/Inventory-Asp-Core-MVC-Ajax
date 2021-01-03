using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using Inventory_Asp_Core_MVC_Ajax.Businesses.common;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using InventoryProject.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class StorageController : Controller
    {
        private readonly IStorageBiz storageBiz;
        private readonly IRepository repository;

        public StorageController(IStorageBiz storageBiz, IRepository repository)
        {
            this.storageBiz = storageBiz;
            this.repository = repository;
        }




        //#region Storages

        //[HttpGet, ActionName("Storages")]
        //public async Task<IActionResult> Storages([FromQuery] QueryModel queryModel)
        //{

        //    return View(await GetSearchStorage(queryModel.PageNumber, queryModel.Searchby));
        //}

        //private async Task<StorageFilterModel> GetSearchStorage(int? page = null, string searchQuery = null)
        //{
        //    var storageResults = await storageBiz.List(new PagingModel()
        //    {
        //        PageNumber = (page == null || page <= 0 ? 1 : page.Value) - 1,
        //        PageSize = 8,
        //        Sort = "LastModified",
        //        SortDirection = SortDirection.DESC
        //    }, searchQuery);
        //    if (!storageResults.Success)
        //        return null;
        //    return new StorageFilterModel()
        //    {
        //        StorageModels = new StaticPagedList<StorageModel>(storageResults.Items,
        //        storageResults.PageNumber + 1, storageResults.PageSize, (int)storageResults.TotalCount),
        //        // SearchQuery = searchQuery
        //    };
        //}

        //#endregion


        #region AddOrEditStorage

        [HttpGet, ActionName("AddOrEditStorage")]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new StorageModel());
            }
            else
            {
                var storageResult = await storageBiz.GetById(id);
                if (!storageResult.Success)
                    return NotFound();
                return View(storageResult.Data);
            }
        }


        [HttpPost, ActionName("AddOrEditStorage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind] StorageModel model)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return Json(this.HtmlReponse(view: "AddOrEditStorage", model, 
                    Result.Failed(Error.WithCode(ErrorCodes.InvalidModel))));
            if (id == 0)
            {
                var result = await storageBiz.Add(model);
                if (!result.Success)
                    return Json(this.HtmlReponse(view: "AddOrEditStorage", model, result));
            }
            else
            {
                var result = await storageBiz.Edit(model);
                if (!result.Success)
                    return Json(this.HtmlReponse(view: "AddOrEditStorage", model, result));
            }
            return Json(this.HtmlReponse());
        }

        #endregion

        #region Delete

        [HttpGet, ActionName("Delete")]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await storageBiz.Delete(id);
            if (!result.Success)
                return Json(this.HtmlReponse(result: result));
            return Json(this.HtmlReponse());
        }

        #endregion

        #region IsNameInUse

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> IsNameAvailable(string name) =>
            (await storageBiz.IsNameInUse(name)).Data ? Json($"Name {name} is already in use.") : Json(true);

        #endregion



        #region Storages

        [HttpGet]
        public IActionResult Storages()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Storages([FromBody] DtParameters dtParameters)
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
            var result = (await repository.ListAsNoTrackingAsync<Storage>()).Data;
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => (r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper())) ||
                                           r.Address != null && r.Address.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.City != null && r.City.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Phone != null && r.Phone.ToUpper().Contains(searchBy.ToUpper())).ToList();
            }
            result = orderAscendingDirection ?
                result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() :
                result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            var filteredResultsCount = result.Count();
            var totalResultsCount = repository.GetCurrentContext().Set<Storage>().Count();
            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }




        private async Task<StorageFilterModel> GetSearchStorage(int? page = null, string searchQuery = null)
        {
            var storageResults = await storageBiz.List(new PagingModel()
            {
                PageNumber = (page == null || page <= 0 ? 1 : page.Value) - 1,
                PageSize = 8,
                Sort = "LastModified",
                SortDirection = SortDirection.DESC
            }, searchQuery);
            if (!storageResults.Success)
                return null;
            return new StorageFilterModel()
            {
                StorageModels = new StaticPagedList<StorageModel>(storageResults.Items,
                storageResults.PageNumber + 1, storageResults.PageSize, (int)storageResults.TotalCount),
                // SearchQuery = searchQuery
            };
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] DtParameters dtParameters)
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
            var result = (await repository.ListAsNoTrackingAsync<Storage>()).Data;
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => (r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper())) ||
                                           r.Address != null && r.Address.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.City != null && r.City.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Phone != null && r.Phone.ToUpper().Contains(searchBy.ToUpper())).ToList();
            }
            result = orderAscendingDirection ?
                result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() :
                result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            var filteredResultsCount = result.Count();
            var totalResultsCount = repository.GetCurrentContext().Set<Storage>().Count();
            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }
    }

    public static class LinqExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember, DtOrderDir ascendingDirection)
        {
            var param = Expression.Parameter(typeof(T), "c");
            var body = orderByMember.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);
            var queryable = ascendingDirection == DtOrderDir.Asc ?
                (IOrderedQueryable<T>)Queryable.OrderBy(query.AsQueryable(), (dynamic)Expression.Lambda(body, param)) :
                (IOrderedQueryable<T>)Queryable.OrderByDescending(query.AsQueryable(), (dynamic)Expression.Lambda(body, param));
            return queryable;
        }
    }
}
