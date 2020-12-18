using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Syncfusion.Pdf;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface IReportBiz
    {
        Task<Result<PdfDocument>> GenerateProductPdfReport(ProductReportModel model);
        Task<Result<byte[]>> GenerateProductExcelReport(ProductReportModel model);
        Task<Result<string>> GenerateProductCsvReport(ProductReportModel model);
    }
}