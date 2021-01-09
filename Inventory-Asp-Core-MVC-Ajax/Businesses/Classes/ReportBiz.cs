//using AspNetCore.Lib.Models;
//using AspNetCore.Lib.Services;
//using AutoMapper;
//using ClosedXML.Excel;
//using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
//using Inventory_Asp_Core_MVC_Ajax.EFModels;
//using Inventory_Asp_Core_MVC_Ajax.Models;
//using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
//using RazorLight;
//using Syncfusion.HtmlConverter;
//using Syncfusion.Pdf;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
//{
//    public class ReportBiz : IReportBiz
//    {
//        private readonly IRepository repository;
//        private readonly IMapper mapper;
//        private readonly ILogger logger;

//        public ReportBiz(IRepository repository, IMapper mapper, ILogger logger)
//        {
//            this.repository = repository;
//            this.mapper = mapper;
//            this.logger = logger;
//        }

//        #region GenerateProductPdfReport

//        public Task<Result<PdfDocument>> GenerateProductPdfReport(ProductReportModel model) =>
//            Result<PdfDocument>.TryAsync(async () =>
//            {
//                var result = await GetProductListFromDB(model);
//                if (!result.Success)
//                {
//                    return Result<PdfDocument>.Failed(Error.WithCode(ErrorCodes.ErrorInProductReport));
//                }
//                var productModels = result.Data.Select(p => mapper.Map<Product, ProductModel>(p)).ToList();

//                //Initialize HTML to PDF converter 
//                HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter()
//                {
//                    //Assign WebKit settings to HTML converter
//                    ConverterSettings = new WebKitConverterSettings
//                    {
//                        //Set WebKit path
//                        WebKitPath = $"{Directory.GetCurrentDirectory()}" +
//                        $"{Path.DirectorySeparatorChar}bin" +
//                        $"{Path.DirectorySeparatorChar}Debug" +
//                        $"{Path.DirectorySeparatorChar}netcoreapp3.1" +
//                        $"{Path.DirectorySeparatorChar}QtBinariesWindows"
//                    }
//                };

//                string templatesPath = $"{Directory.GetCurrentDirectory()}" +
//                    $"{Path.DirectorySeparatorChar}Api" +
//                    $"{Path.DirectorySeparatorChar}Resources";

//                RazorLightEngine engine = new RazorLightEngineBuilder().UseFileSystemProject(templatesPath)
//                    .UseMemoryCachingProvider().Build();
//                string resultt = await engine.CompileRenderAsync("ProductPdfReport.cshtml", productModels);

//                var pdfDocument = htmlToPdfConverter.Convert(resultt, $"www.google.com");
//                logger.Info("Product pdf report generated");
//                return Result<PdfDocument>.Successful(pdfDocument);
//            });

//        #endregion

//        #region GenerateProductExcelReport

//        public Task<Result<byte[]>> GenerateProductExcelReport(ProductReportModel model) =>
//            Result<byte[]>.TryAsync(async () =>
//            {
//                Result<IList<Product>> result = await GetProductListFromDB(model);
//                if (!result.Success)
//                {
//                    return Result<byte[]>.Failed(Error.WithCode(ErrorCodes.ErrorInProductReport));
//                }
//                var productModels = result.Data.Select(p => mapper.Map<Product, ProductModel>(p)).ToList();
//                byte[] content;
//                using (var workbook = new XLWorkbook())
//                {
//                    var worksheet = workbook.Worksheets.Add("Products");
//                    var currentRow = 1;
//                    worksheet.Cell(currentRow, 1).Value = "Name";
//                    worksheet.Cell(currentRow, 2).Value = "Quantity";
//                    worksheet.Cell(currentRow, 3).Value = "Price";
//                    worksheet.Cell(currentRow, 4).Value = "IsAvailable";
//                    foreach (var p in productModels)
//                    {
//                        currentRow++;
//                        worksheet.Cell(currentRow, 1).Value = p.Name;
//                        worksheet.Cell(currentRow, 2).Value = p.Quantity;
//                        worksheet.Cell(currentRow, 3).Value = p.Price;
//                        worksheet.Cell(currentRow, 4).Value = p.IsAvailable;
//                    }
//                    using var stream = new MemoryStream();
//                    workbook.SaveAs(stream);
//                    content = stream.ToArray();
//                }
//                logger.Info("Product excel report generated");
//                return Result<byte[]>.Successful(content);
//            });

//        #endregion

//        #region GenerateProductCsvReport

//        public Task<Result<string>> GenerateProductCsvReport(ProductReportModel model) =>
//            Result<string>.TryAsync(async () =>
//            {
//                Result<IList<Product>> result = await GetProductListFromDB(model);
//                if (!result.Success)
//                {
//                    return Result<string>.Failed(Error.WithCode(ErrorCodes.ErrorInProductReport));
//                }
//                var productModels = result.Data.Select(p => mapper.Map<Product, ProductModel>(p)).ToList();

//                var builder = new StringBuilder();
//                builder.AppendLine("Name,Quantity,Price,IsAvailable");
//                foreach (var p in productModels)
//                {
//                    builder.AppendLine($"{p.Name},{p.Quantity},{p.Price},{p.IsAvailable}");
//                }
//                logger.Info("Product excel report generated");
//                return Result<string>.Successful(builder.ToString());
//            });

//        #endregion

//        #region GetProductListFromDB

//        private Task<Result<IList<Product>>> GetProductListFromDB(ProductReportModel model) =>
//           Result<IList<Product>>.TryAsync(async () => await
//           repository.ListAsNoTrackingAsync<Product>(p =>
//                   //(model.IsAvailable == null || p.IsAvailable == model.IsAvailable) &&
//                   //(model.MaxPrice == null || p.Price <= model.MaxPrice) &&
//                   //(model.MinPrice == null || p.Price >= model.MinPrice) &&
//                   //(model.MaxQuantity == null || p.Quantity <= model.MaxQuantity) &&
//                   //(model.MinQuantity == null || p.Quantity >= model.MinQuantity) &&
//                   //(model.MaxPrice == null || p.Price <= model.MaxPrice) &&
//                   (model.StorageId == null || p.StorageId == model.StorageId) &&
//                   (model.SupplierId == null || p.SupplierId == model.SupplierId),
//                  p => p.Storage, p => p.Supplier));

//        #endregion
//    }
//}
