namespace Inventory_Asp_Core_MVC_Ajax.Models
{
    public class ErrorCodes
    {
        #region Auth
        public const int UserNameAlreadyExists = 1001;
        public const int UserEmailAlreadyExists = 1002;
        public const int UserCreationFailed = 1003;
        public const int UserLoginFailed = 1004;
        #endregion

        #region Product
        public const int ProductNotFoundById = 2001;
        public const int ProductsNotFound = 2002;
        public const int ProductsNotFoundByStoreId = 2003;
        public const int ProductJoinedWithStoreNotFoundByProductId = 2004;
        #endregion

        #region Storage
        public const int StorageNotFoundById = 3001;
        public const int StoragesNotFound = 3002;
        public const int StorageProductsNotFound = 3003;
        #endregion

        #region Image
        public const int ImagesNotFoundByProductId = 4001;
        public const int ImageNotFoundById = 4002;
        #endregion
    }
}
