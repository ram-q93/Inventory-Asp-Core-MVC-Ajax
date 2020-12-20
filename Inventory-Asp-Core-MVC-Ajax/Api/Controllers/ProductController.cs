using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Extensions;
using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductBiz productBiz;
        private readonly IImageBiz imageBiz;

        public ProductController(
            IProductBiz productBiz,
            IImageBiz imageBiz)
        {
            this.productBiz = productBiz;
            this.imageBiz = imageBiz;
        }


        #region Products

        [HttpGet, ActionName("Products")]
        public async Task<IActionResult> Products(int storageId, string query, int? page = null)
            => View(await GetProductFilterModel(storageId, query, page));

        private async Task<ProductFilterModel> GetProductFilterModel(int storageId, string query = null, int? page = null)
        {
            var result = await productBiz.GetStoragePagedListProductFilteredBySearchQuery(storageId, page, query);
            if (result.TotalCount == 0)
                return new ProductFilterModel()
                {
                    ProductPagedList = new StaticPagedList<ProductModel>(new[] { new ProductModel() { StorageId = storageId } },
                    result.PageNumber + 1, result.PageSize, (int)result.TotalCount),
                    SearchQuery = query
                };
            return new ProductFilterModel()
            {
                ProductPagedList = new StaticPagedList<ProductModel>(result.Items,
                result.PageNumber + 1, result.PageSize, (int)result.TotalCount),
                SearchQuery = query
            };
        }

        #endregion

        #region AddOrEditProduct

        [HttpGet, ActionName("AddOrEditProduct")]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id, int storageId)
        {
            if (id == 0)
            {
                return View(new ProductModel() { StorageId = storageId });
            }
            else
            {
                var productResult = await productBiz.GetById(id);
                if (!productResult.Success)
                    return NotFound();
                return View(productResult.Data);
            }
        }


        [HttpPost, ActionName("AddOrEditProduct")]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit([Bind] ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return Respo(false, "AddOrEditProduct", model);
            }
            if (model.Id == 0) //Add
            {
                var result = await productBiz.Add(model);
                if (!result.Success)
                    return Respo(false, "AddOrEditProduct", model, result);
            }
            else //Edit
            {
                var result = await productBiz.Edit(model);
                if (!result.Success)
                    return Respo(false, "AddOrEditProduct", model, result);
            }
            return Respo(success: true, view: "_Products", model: await GetProductFilterModel(model.StorageId));
        }

        #endregion

        #region Delete

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int storageId)
        {
            var result = await productBiz.Delete(id);
            if (!result.Success)
                return Respo(false, "_Products", null, result);
            return Respo(true, "_Products", await GetProductFilterModel(storageId));
        }

        #endregion

        #region ProductDetails

        [HttpGet, ActionName("ProductDetails")]
        public async Task<IActionResult> Details(int id)
        {
            var productResult = await productBiz.Details(id);
            return View(productResult.Data);
        }

        #endregion

        private IActionResult Respo(bool success, string view, object model, Result result = null) => Json(new
        {
            success,
            error = success ? "" : $"Error {result?.Error?.Code}",
            html = this.RenderRazorViewToString(view, model)
        });
    }
}

