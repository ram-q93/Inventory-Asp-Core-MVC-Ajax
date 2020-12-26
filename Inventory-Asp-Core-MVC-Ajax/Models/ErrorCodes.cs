namespace Inventory_Asp_Core_MVC_Ajax.Models
{
    public class ErrorCodes
    {
        public const int InvalidModel = 1005;

        #region Product
        public const int ProductNotFoundById = 2001;
        public const int ProductsNotFound = 2002;
        public const int ProductsNotFoundByStoreId = 2003;
        public const int ProductJoinedWithStoreNotFoundByProductId = 2004;
        public const int PagedListFilteredBySearchQueryNotFound = 2005;
        public const int ProductDetailsNotFoundById = 2006;
        public const int ProductNameAlreadyExists = 2007;
        #endregion

        #region Storage
        public const int StorageNotFoundById = 3001;
        public const int StoragesNotFound = 3002;
        public const int StorageProductsNotFound = 3003;
        public const int StorageNameAlreadyExists = 3004;
        #endregion

        #region Image
        public const int ImagesNotFoundByProductId = 4001;
        public const int ImageNotFoundById = 4002;
        #endregion

        #region Supplier
        public const int SuppliersNotFound = 5001;
        public const int SupplierNotFoundById = 5002;
        public const int SupplierDetailsNotFoundById = 5003;
        public const int EnabaledSuppliersNotFoundForSelectList = 5004;
        public const int SupplierNameAlreadyExists = 5005;
        #endregion

        #region Report
        public const int ErrorInProductReport = 6001;
        #endregion

        #region Auth
        public const int UserNameAlreadyExists = 4040;
        public const int UserEmailAlreadyExists = 4041;
        public const int UserCreationFailed = 4042;
        public const int UserLoginFailed = 4043;
        #endregion
    }
}
