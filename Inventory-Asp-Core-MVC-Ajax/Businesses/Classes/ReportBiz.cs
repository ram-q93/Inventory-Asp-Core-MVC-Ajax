using AspNetCore.Lib.Models;
using AspNetCore.Lib.Services.Interfaces;
using AutoMapper;
using ClosedXML.Excel;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Core;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RazorLight;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.Collections.Generic;
using System.Drawing;
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
                
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var stream = new MemoryStream();
                using var package = new ExcelPackage(stream);

                var ws = package.Workbook.Worksheets.Add("Product Report Sheet");
                var range = ws.Cells["A2"].LoadFromCollection(productModels, true);
                range.AutoFitColumns();

                // Formats the header
                ws.Cells["A1"].Value = "Product Report";
                ws.Cells["A1:M1"].Merge = true;
                ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Row(1).Style.Font.Size = 24;
                ws.Row(1).Style.Font.Color.SetColor(Color.Blue);

                ws.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Row(2).Style.Font.Bold = true;
                ws.Column(3).Width = 20;
                await package.SaveAsync();

                _logger.Info("Product excel report generated");
                return Result<byte[]>.Successful(stream.ToArray());
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
                builder.AppendLine("Name,Code,Quantity,UnitePrice,Enabled,Category,Storage,Supplier");
                foreach (var p in productModels)
                {
                    builder.AppendLine($"{p.Name},{p.Code},{p.Quantity},{p.UnitePrice}," +
                        $"{p.Enabled},{p.CategoryName},{p.StorageName},{p.SupplierCompanyName}");
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
                   Quantity = p.Quantity,
                   Enabled = p.Enabled,
                   CategoryName = p.Category.Name,
                   StorageName = p.Storage.Name,
                   SupplierCompanyName = p.Supplier.CompanyName
               }).OrderBy(p=>p.Name).ToListAsync();

              return Result<IList<ProductModel>>.Successful(result);
          });

        #endregion
    }
}
