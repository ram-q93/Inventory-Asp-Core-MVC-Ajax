using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Common;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductBiz _productBiz;
        private readonly IReportBiz _reportBiz;

        public ProductController(IProductBiz productBiz, IReportBiz reportBiz)
        {
            _productBiz = productBiz;
            _reportBiz = reportBiz;
        }

        #region Products

        [HttpGet]
        public IActionResult Products() => View();


        [HttpPost]
        public async Task<IActionResult> Products([FromBody] DataTableParameters dtParameters)
            => Json((await _productBiz.List(dtParameters)).Data);

        #endregion

        #region AddOrEditProduct

        [HttpGet, ActionName("AddOrEditProduct")]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new ProductModel());
            }
            else
            {
                var productResult = await _productBiz.GetById(id);
                if (!productResult.Success)
                    return NotFound();
                return View(productResult.Data);
            }
        }

        [HttpPost, ActionName("AddOrEditProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind] ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(this.HtmlReponse(view: "AddOrEditProduct", model,
                        Result.Failed(Error.WithCode(ErrorCodes.InvalidModel))));
            }
            if (model.Id == 0)
            {
                var result = await _productBiz.Add(model);
                if (!result.Success)
                    return Json(this.HtmlReponse(view: "AddOrEditProduct", model, result));
            }
            else
            {
                var result = await _productBiz.Edit(model);
                if (!result.Success)
                    return Json(this.HtmlReponse(view: "AddOrEditProduct", model, result));
            }
            return Json(this.HtmlReponse());
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productBiz.Delete(id);
            if (!result.Success)
                return Json(this.HtmlReponse(result: result));
            return Json(this.HtmlReponse());
        }

        #endregion   

        #region IsNameAvailable

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> IsNameAvailable(string name) =>
            (await _productBiz.IsNameInUse(name)).Data ? Json($"Name {name} is already in use.") : Json(true);

        #endregion

        #region Details

        [HttpGet, ActionName("Details")]
        [NoDirectAccess]
        public async Task<IActionResult> Details(int id)
        {

            var result = await _productBiz.Details(id);
            if (!result.Success)
                return NotFound();
            return View(result.Data);

        }

        #endregion

        #region Report

        [HttpGet]
        public IActionResult Report() => View(new ProductReportModel());


        [HttpPost, ActionName("csv")]
        public async Task<IActionResult> ProductCsvReport(ProductReportModel model)
        {
            var result = await _reportBiz.GenerateProductCsvReport(model);
            if (!result.Success)
            {
                return null;
            }
            return File(Encoding.UTF8.GetBytes(result.Data), "text/csv", "Sample.csv");
        }


        #endregion
    }
}

