

using PagedList.Core.Mvc;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Common
{
    public class SitePagedListRenderOptions
    {
        public static PagedListRenderOptions Boostrap4
        {
            get
            {
                var option = PagedListRenderOptions.Bootstrap4Full;

                option.MaximumPageNumbersToDisplay = 8;

                return option;
            }
        }
    }
}
