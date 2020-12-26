using AspNetCore.Lib.Extensions;
using AspNetCore.Lib.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.common
{
    public static class HtmlReponseExtension
    {
        public static object HtmlReponse(this Controller controller, string view,
            object model, Result result = null) =>
            new
            {
                success = result != null && result.Success,
                error = result == null ? "" : $"Error {result?.Error?.Code}",
                html = controller.RenderRazorViewToString(view, model)
            };

    }
}
