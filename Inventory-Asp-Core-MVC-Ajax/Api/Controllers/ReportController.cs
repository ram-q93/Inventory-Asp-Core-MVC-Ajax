using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using System.IO;
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
    }
}
