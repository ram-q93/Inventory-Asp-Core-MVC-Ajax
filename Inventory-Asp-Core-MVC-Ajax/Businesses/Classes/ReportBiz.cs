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
        private readonly ILogger logger;

        public ReportBiz(IRepository repository, IMapper mapper,ILogger logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public Task<Result<PdfDocument>> GenerateProductPdfReport(ProductReportModel model) =>
            Result<PdfDocument>.TryAsync(async () =>
            {
                var result = await repository.ListAsNoTrackingAsync<Product>(p =>
                    (model.IsAvailable == null || p.IsAvailable == model.IsAvailable) &&
                    (model.MaxPrice == null || p.Price <= model.MaxPrice) &&
                    (model.MinPrice == null || p.Price >= model.MinPrice) &&
                    (model.MaxQuantity == null || p.Quantity <= model.MaxQuantity) &&
                    (model.MinQuantity == null || p.Quantity >= model.MinQuantity) &&
                    (model.MaxPrice == null || p.Price <= model.MaxPrice) &&
                    (model.StorageId == null || p.StorageId == model.StorageId) &&
                    (model.SupplierId == null || p.SupplierId == model.SupplierId),
                    p => p.Storage, p => p.Supplier);
                if (!result.Success)
                {
                    return Result<PdfDocument>.Failed(Error.WithCode(ErrorCodes.ErrorInProductReport));
                }
                var productModels = result.Data.Select(p => mapper.Map<Product, ProductModel>(p)).ToList();

                //Initialize HTML to PDF converter 
                HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter()
                {
                    //Assign WebKit settings to HTML converter
                    ConverterSettings = new WebKitConverterSettings
                    {
                        //Set WebKit path
                        WebKitPath = $"{Directory.GetCurrentDirectory()}" +
                        $"{Path.DirectorySeparatorChar}bin" +
                        $"{Path.DirectorySeparatorChar}Debug" +
                        $"{Path.DirectorySeparatorChar}netcoreapp3.1" +
                        $"{Path.DirectorySeparatorChar}QtBinariesWindows"
                    }
                };

                string templatesPath = $"{Directory.GetCurrentDirectory()}" +
                    $"{Path.DirectorySeparatorChar}Api" +
                    $"{Path.DirectorySeparatorChar}Resources";

                RazorLightEngine engine = new RazorLightEngineBuilder().UseFileSystemProject(templatesPath)
                    .UseMemoryCachingProvider().Build();
                string resultt = await engine.CompileRenderAsync("ProductPdfReport.cshtml", productModels);

                var pdfDocument = htmlToPdfConverter.Convert(resultt, $"www.google.com");
                logger.Info("Product Pdf Report Generated");
                return Result<PdfDocument>.Successful(pdfDocument);
            });
    }
}
