using AspNetCore.Lib.Services;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Products(int? storageId)
        {
            try
            {
                if (storageId == null)
                    return NotFound();
                var storageWithProductsResult = await productBiz.StorageJoinedToProductListByStoreId((int)storageId);
                if (!storageWithProductsResult.Success)
                    return NotFound();
                if (storageWithProductsResult.Data.ProductModels.Count == 0)
                    storageWithProductsResult.Data.ProductModels = new[] { new ProductModel() };
                return View(storageWithProductsResult.Data);
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
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

    }
}

