
using AspNetCore.Lib.Services;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageBiz imageBiz;
        private readonly ILogger logger;

        public ImageController(IImageBiz imageBusiness, ILogger logger)
        {
            this.imageBiz = imageBusiness;
            this.logger = logger;
        }

        #region Images

        //[HttpGet, ActionName("ProductImages")]
        //public async Task<IActionResult> Images(int? productId)
        //{
        //    var stProdsAndImgsResult = await imageBiz.GetStoreProductsAndImages((int)productId);
        //    if (!stProdsAndImgsResult.Success)
        //        return NotFound();
        //    if (stProdsAndImgsResult.Data.ProductModels.Count == 0)
        //    {
        //        stProdsAndImgsResult.Data.ProductModels = new[] { new ProductModel() };
        //        if (stProdsAndImgsResult.Data.ProductModels.First().ImageModels.Count == 0)
        //        {
        //            stProdsAndImgsResult.Data.ProductModels.First().ImageModels = new[] { new ImageModel() };
        //        }
        //    }
        //    return View(stProdsAndImgsResult.Data);
        //}

        #endregion

        #region Create

        //[HttpGet, ActionName("CreateImage")]
        //public IActionResult Create(int? productId)
        //{
        //    if (productId == null)
        //        return NotFound();
        //    return View(new ImageModel() { ProductId = (int)productId });
        //}

        //[HttpPost, ActionName("CreateImage")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind] ImageModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return View(model);
        //        var imageModels = new List<ImageModel>();
        //        foreach (var file in Request.Form.Files)
        //        {
        //            MemoryStream ms = new MemoryStream();
        //            file.CopyTo(ms);
        //            imageModels.Add(new ImageModel()
        //            {
        //                Title = file.FileName,
        //                Data = ms.ToArray(),
        //                ProductId = model.ProductId
        //            });
        //            ms.Close();
        //            ms.Dispose();
        //        }
        //        if (imageModels.Count == 0)
        //            return View(model);
        //        var result = await imageBiz.AddImages(imageModels);
        //        if (!result.Success)
        //            return View(model);
        //        logger.Info($"New Image Added ");
        //        return RedirectToAction("ProductImages", new { productId = model.ProductId });
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Exception(e);
        //        return NotFound();
        //    }

        //}

        #endregion

        #region Delete

        [HttpGet, ActionName("DeleteImage")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();
                var result = await imageBiz.GetById((int)id);
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

        //[HttpPost, ActionName("DeleteImage")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete([Bind] ImageModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return View(model);
        //        var result = await imageBiz.Delete(model.Id);
        //        if (!result.Success)
        //            return View(model);
        //        logger.Info($"Image Deleted  (id={model.Id})");
        //        return RedirectToAction("ProductImages", new { productId = model.ProductId });
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Exception(e);
        //        return NotFound();
        //    }
        //}

        #endregion

    }
}
