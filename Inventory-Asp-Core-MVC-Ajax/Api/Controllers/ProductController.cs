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

        #region Create

        [HttpGet, ActionName("CreateProduct")]
        public IActionResult Create(int? storeId)
        {
            if (storeId == null)
                return NotFound();
            return View(new ProductModel() { StorageId = (int)storeId });
        }


        [HttpPost, ActionName("CreateProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] ProductModel productModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(productModel);
                var result = await productBiz.Add(productModel);
                if (!result.Success)
                    return View(productModel);
                return RedirectToAction("Products", new { productModel.StorageId });
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
        }

        #endregion

        #region Edit

        [HttpGet, ActionName("EditProduct")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();
                var result = await productBiz.GetById((int)id);
                if (!result.Success)
                    return NotFound();
                return View(model: result.Data);
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
        }

        [HttpPost, ActionName("EditProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind] ProductModel productModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(productModel);
                var result = await productBiz.Edit(productModel);
                if (!result.Success)
                    return View(productModel);
                logger.Info($"Product Edited  {productModel}");
                return RedirectToAction("Products", new { productModel.StorageId });
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
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
                return response(false, "AddOrEditProduct", model);
            if (id == 0)
            {
                var result = await productBiz.Add(model);
                if (!result.Success)
                    return response(false, "AddOrEditProduct", model, result);
            }
            else
            {
                var result = await productBiz.Edit(model);
                if (!result.Success)
                    return response(false, "AddOrEditProduct", model, result);
            }
            return response(success: true, view: "_Products", model: await GetProductFilterModel(model.StorageId));
        }

        #endregion

        #region Delete

        [HttpGet, ActionName("DeleteProduct")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();
                var result = await productBiz.GetById((int)id);
                if (!result.Success || result?.Data == null)
                    return NotFound();
                return View(model: result.Data);
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind] ProductModel productModel)
        {
            try
            {
                if (productModel?.Id == null)
                    return NotFound();
                var result = await productBiz.Delete((int)productModel.Id);
                if (!result.Success)
                    return RedirectToAction("DeleteProduct", new { productModel.Id });
                return RedirectToAction("Products", new { productModel.StorageId });
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
        }

        #endregion


        private IActionResult response(bool success, string view, object model = null, Result result = null) => Json(new
        {
            success,
            error = success ? "" : $"Error {result?.Error?.Code}",
            html = this.RenderRazorViewToString(view, model)
        });
    }
}

