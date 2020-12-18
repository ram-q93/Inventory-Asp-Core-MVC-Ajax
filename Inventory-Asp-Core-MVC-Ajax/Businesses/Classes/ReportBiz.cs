using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services;
using AutoMapper;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using RazorLight;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class ReportBiz : IReportBiz
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public ReportBiz(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task<Result<PdfDocument>> GenerateProductPdfReport(ProductReportModel model) =>
            Result<PdfDocument>.TryAsync(async () =>
            {
                var result = await repository.ListAsNoTrackingAsync<Product>(
                    //p =>
                    //(model.IsAvailable != null || p.IsAvailable == model.IsAvailable) &&
                    //(model.MaxPrice != null || p.Price <= model.MaxPrice) &&
                    //(model.MinPrice != null || p.Price >= model.MinPrice) &&
                    //(model.MaxQuantity != null || p.Quantity <= model.MaxQuantity) &&
                    //(model.MinQuantity != null || p.Quantity >= model.MinQuantity) &&
                    //(model.MaxPrice != null || p.Price <= model.MaxPrice) &&
                    //(model.StorageId != null || p.StorageId == model.StorageId) &&
                    //(model.SupplierId != null || p.SupplierId == model.SupplierId),
                    p => p.Storage, p => p.Supplier);
                if (!result.Success)
                {
                    return Result<PdfDocument>.Failed(Error.WithCode(ErrorCodes.ErrorInProductReport));
                }

                //Initialize HTML to PDF converter 
                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
                WebKitConverterSettings settings = new WebKitConverterSettings();
                //Set WebKit path
                settings.WebKitPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}QtBinariesWindows";
                //Assign WebKit settings to HTML converter
                htmlConverter.ConverterSettings = settings;

                string templatesPath = $"{Directory.GetCurrentDirectory()}" +
                    $"{Path.DirectorySeparatorChar}Api" +
                    $"{Path.DirectorySeparatorChar}Resources";
                var engine = new RazorLightEngineBuilder().UseFileSystemProject(templatesPath)
                    .UseMemoryCachingProvider().Build();
                var productModels = result.Data.Select(p => mapper.Map<Product, ProductModel>(p)).ToList();
                string resultt = await engine.CompileRenderAsync("ProductPdfReport.cshtml", productModels);

                var pdfDocument = new HtmlToPdfConverter().Convert(resultt, $"www.google.com");
                return Result<PdfDocument>.Successful(pdfDocument);
            });
    }
}
