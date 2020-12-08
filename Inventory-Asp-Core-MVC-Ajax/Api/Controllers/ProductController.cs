using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Extensions;
using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductBiz productBiz;
        private readonly ISerializer serializer;
        private readonly ILogger logger;

        public ProductController(
            IProductBiz productBusiness,
            ISerializer serializer,
            ILogger logger)
        {
            this.productBiz = productBusiness;
            this.serializer = serializer;
            this.logger = logger;
        }


        #region Products

        [HttpGet, ActionName("Products")]
        public async Task<IActionResult> Products(int storageId, string query, int? page = null)
            => View(await GetProductFilterModel(storageId, query, page));

        private async Task<ProductFilterModel> GetProductFilterModel(int storageId, string query = null, int? page = null)
        {
            var ProductResults = await productBiz.GetStoragePagedListProductFilteredBySearchQuery(storageId, page, query);
            return new ProductFilterModel()
            {
                ProductPagedList = new StaticPagedList<ProductModel>(ProductResults.Items,
                ProductResults.PageNumber + 1, ProductResults.PageSize, (int)ProductResults.TotalCount),
                SearchQuery = query
            };
        }

        #endregion

        #region AddOrEditProduct

        [HttpGet, ActionName("AddOrEditProduct")]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0, int storageId = 0)
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
        public async Task<IActionResult> AddOrEdit(int id, [Bind] ProductModel model)
        {
            if (!ModelState.IsValid)
                return Respo(false, "AddOrEditProduct", model);
            if (id == 0)
            {
                var result = await productBiz.Add(model);
                if (!result.Success)
                    return Respo(false, "AddOrEditProduct", model, result);
            }
            else
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

