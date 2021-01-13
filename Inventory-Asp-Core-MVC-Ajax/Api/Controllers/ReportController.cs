using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportBiz reportBiz;

        public ReportController(IReportBiz reportBiz)
        {
            this.reportBiz = reportBiz;
        }

        [HttpGet, ActionName("ProductReport")]
        public IActionResult ProductReport() => View();

        #region product-pdf

        [HttpGet, ActionName("product-pdf")]
        public async Task<IActionResult> ProductPDFReport(ProductReportModel model)
        {
            var result = await reportBiz.GenerateProductPdfReport(model);
            if (!result.Success || result.Data == null)
            {
                return null;
            }
            PdfDocument pdfDocument = result.Data;
            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Sample.pdf");
        }

        #endregion

        #region product-excel

        [HttpGet, ActionName("product-excel")]
        public async Task<IActionResult> ProductExcelReport(ProductReportModel model)
        {
            var result = await reportBiz.GenerateProductExcelReport(model);
            if (!result.Success)
            {
                return null;
            }
            return File(result.Data,
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              "Sample.xlsx");
        }

        #endregion

        #region product-csv

        [HttpPost, ActionName("product-csv")]
        public async Task<IActionResult> ProductCsvReport(ProductReportModel model)
        {
            var result = await reportBiz.GenerateProductCsvReport(model);
            if (!result.Success)
            {
                return null;
            }
            return File(Encoding.UTF8.GetBytes(result.Data), "text/csv", "Sample.csv");
        }

        #endregion
    }
}
