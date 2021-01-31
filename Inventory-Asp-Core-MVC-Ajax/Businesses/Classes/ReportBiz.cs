using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services.Interfaces;
using AutoMapper;
using ClosedXML.Excel;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Core;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using Microsoft.EntityFrameworkCore;
using RazorLight;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Classes
{
    public class ReportBiz : IReportBiz
    {
        private readonly IInventoryDbContext _context;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ReportBiz(IInventoryDbContext context, IRepository repository, IMapper mapper, ILogger logger)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        #region GenerateProductPdfReport

        public Task<Result<PdfDocument>> GenerateProductPdfReport(ProductReportModel model) =>
            Result<PdfDocument>.TryAsync(async () =>
            {
                Result<IList<ProductModel>> result = await GetProductListFromDB(model);
                if (!result.Success)
                {
                    return Result<PdfDocument>.Failed(Error.WithCode(ErrorCodes.ErrorInProductReport));
                }
                var productModels = result.Data;

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
                _logger.Info("Product pdf report generated");
                return Result<PdfDocument>.Successful(pdfDocument);
            });

        #endregion

        #region GenerateProductExcelReport

        public Task<Result<byte[]>> GenerateProductExcelReport(ProductReportModel model) =>
            Result<byte[]>.TryAsync(async () =>
            {
                Result<IList<ProductModel>> result = await GetProductListFromDB(model);
                if (!result.Success)
                {
                    return Result<byte[]>.Failed(Error.WithCode(ErrorCodes.ErrorInProductReport));
                }
                var productModels = result.Data;
                byte[] content;
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Products");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Name";
                    worksheet.Cell(currentRow, 2).Value = "Quantity";
                    worksheet.Cell(currentRow, 3).Value = "UnitePrice";
                    worksheet.Cell(currentRow, 4).Value = "Enabled";
                    foreach (var p in result.Data)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = p.Name;
                        worksheet.Cell(currentRow, 2).Value = p.Quantity;
                        worksheet.Cell(currentRow, 3).Value = p.UnitePrice;
                        worksheet.Cell(currentRow, 4).Value = p.Enabled;
                    }
                    using var stream = new MemoryStream();
                    workbook.SaveAs(stream);
                    content = stream.ToArray();
                }
                _logger.Info("Product excel report generated");
                return Result<byte[]>.Successful(content);
            });

        #endregion

        #region GenerateProductCsvReport

        public Task<Result<string>> GenerateProductCsvReport(ProductReportModel model) =>
            Result<string>.TryAsync(async () =>
            {
                Result<IList<ProductModel>> result = await GetProductListFromDB(model);
                if (!result.Success)
                {
                    return Result<string>.Failed(Error.WithCode(ErrorCodes.ErrorInProductReport));
                }
                var productModels = result.Data;

                var builder = new StringBuilder();
                builder.AppendLine("Name,Quantity,UnitePrice,Enabled");
                foreach (var p in productModels)
                {
                    builder.AppendLine($"{p.Name},{p.Quantity},{p.UnitePrice},{p.Enabled}");
                }
                _logger.Info("Product csv report generated");
                return Result<string>.Successful(builder.ToString());
            });

        #endregion

        #region GetProductListFromDB

        private Task<Result<IList<ProductModel>>> GetProductListFromDB(ProductReportModel model) =>
           Result<IList<ProductModel>>.TryAsync(async () =>
          {
              var result = await _context.Products.AsNoTracking()
              .Where(p =>
                      (model.Enabled == null || p.Enabled == model.Enabled) &&
                      (model.MaxPrice == null || p.UnitePrice <= model.MaxPrice) &&
                      (model.MinPrice == null || p.UnitePrice >= model.MinPrice) &&
                      (model.MaxQuantity == null || p.Quantity <= model.MaxQuantity) &&
                      (model.MinQuantity == null || p.Quantity >= model.MinQuantity) &&
                      (model.StorageId == null || p.StorageId == model.StorageId) &&
                      (model.SupplierId == null || p.SupplierId == model.SupplierId) &&
                      (model.CategoryId == null || p.CategoryId == model.CategoryId))
               .Include(p => p.Category)
               .Include(p => p.Storage)
               .Include(p => p.Supplier)
               .Select(p => new ProductModel
               {
                   Name = p.Name,
                   Code = p.Code,
                   UnitePrice = p.UnitePrice,
                   Enabled = p.Enabled,
                   CategoryName = p.Category.Name,
                   StorageName = p.Storage.Name,
                   SupplierCompanyName = p.Supplier.CompanyName
               }).ToListAsync();

              return Result<IList<ProductModel>>.Successful(result);
          });

        #endregion
    }
}
