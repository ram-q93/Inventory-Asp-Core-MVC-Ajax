using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Syncfusion.Pdf;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface IReportBiz
    {
        Task<Result<PdfDocument>> GenerateProductPdfReport(ProductReportModel model);
    }
}