﻿using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Common;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Core;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class SupplierController : Controller
    {
        public readonly ISupplierBiz _supplierBiz;

        public SupplierController(ISupplierBiz supplierBiz)
        {
            _supplierBiz = supplierBiz;
        }

        #region Suppliers

        [HttpGet]
        public IActionResult Suppliers() => View();

        [HttpPost]
        public async Task<IActionResult> Suppliers([FromBody] DataTableParameters dtParameters)
            => Json((await _supplierBiz.List(dtParameters)).Data);

        #endregion

        #region AddOrEditSupplier

        [HttpGet, ActionName("AddOrEditSupplier")]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new SupplierModel());
            }
            else
            {
                var supplierResult = await _supplierBiz.GetById(id);
                if (!supplierResult.Success)
                    return NotFound();
                return View(supplierResult.Data);
            }
        }

        [HttpPost, ActionName("AddOrEditSupplier")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind] SupplierModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(this.HtmlReponse(view: "AddOrEditSupplier", model,
                        Result.Failed(Error.WithCode(ErrorCodes.InvalidModel))));
            }
            if (model.Id == 0)
            {
                var result = await _supplierBiz.Add(model);
                if (!result.Success)
                    return Json(this.HtmlReponse(view: "AddOrEditSupplier", model, result));
            }
            else
            {
                var result = await _supplierBiz.Edit(model);
                if (!result.Success)
                    return Json(this.HtmlReponse(view: "AddOrEditSupplier", model, result));
            }
            return Json(this.HtmlReponse());
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _supplierBiz.Delete(id);
            if (!result.Success)
                return Json(this.HtmlReponse(result: result));
            return Json(this.HtmlReponse());
        }

        #endregion   

        #region IsNameInUse

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> IsNameAvailable(string name) =>
            (await _supplierBiz.IsNameInUse(name)).Data ? Json(true) : Json(true);

        #endregion

        #region ListByNameAndId
        [HttpGet, ActionName("list-name")]
        public JsonResult ListByNameAndId()
        {
            var result = _supplierBiz.ListName();
            return Json(result.Data);
        }
        #endregion
    }
}
