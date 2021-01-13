namespace Inventory_Asp_Core_MVC_Ajax.Models
{
    public class ErrorCodes
    {
        public const int InvalidModel = 1005;

        #region Product (Prefix = 201)
        public const int ProductNotFoundById = 2011;
        public const int ProductsNotFound = 2012;
        public const int ProductsNotFoundByStoreId = 2013;
        public const int ProductJoinedWithStoreNotFoundByProductId = 2014;
        public const int PagedListFilteredBySearchQueryNotFound = 2015;
        public const int ProductDetailsNotFoundById = 2016;
        public const int ProductNameAlreadyInUse = 2017;
        #endregion

        #region Storage (Prefix = 202)
        public const int StorageNotFoundById = 2021;
        public const int StoragesNotFound = 2022;
        public const int StorageProductsNotFound = 2023;
        public const int StorageNameAlreadyInUse = 2024;
        #endregion

        #region Category (Prefix = 203)
        public const int CategoriesNotFound = 2031; 
        public const int CategoryNotFoundById = 2032;
        public const int CategoryNameAlreadyInUse = 2033;


        #endregion

        #region Supplier (Prefix = 204)
        public const int SuppliersNotFound = 2041;
        public const int SupplierNotFoundById = 2042;
        public const int SupplierDetailsNotFoundById = 2043;
        public const int EnabaledSuppliersNotFoundForSelectList = 2044;
        public const int SupplierNameAlreadyInUse = 2045;
        #endregion

        #region Report (Prefix = 205)
        public const int ErrorInProductReport = 2051;
        #endregion

        #region Dashboard (Prefix = 206)
        public const int ErrorInDashboardStatistics = 2061;
        #endregion

        #region Image (Prefix = 207)
        public const int ErrorInImageContentType = 2071;
        public const int ErrorInImageExtension = 2072;
        public const int ErrorInImageAspectRatio= 2073;
        public const int ErrorInImageCanRead = 2074;
        public const int ErrorInImageSizeExceedingTheLimit = 2075;
        public const int ErrorInImageSizeMinimumLimit = 2076;
        public const int ExceptionInImage= 2077;
        #endregion


        #region Auth
        public const int UserNameAlreadyExists = 4040;
        public const int UserEmailAlreadyExists = 4041;
        public const int UserCreationFailed = 4042;
        public const int UserLoginFailed = 4043;
        #endregion
    }
}
