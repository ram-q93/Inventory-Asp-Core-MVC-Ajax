using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Common
{
    public static class HttpContextExtensions
    {
        public static string GetAntiforgeryToken(this HttpContext httpContext)
        {
            var antiforgery = (IAntiforgery)httpContext.RequestServices.GetService(typeof(IAntiforgery));
            var tokenSet = antiforgery.GetAndStoreTokens(httpContext);
            string requestToken = tokenSet.RequestToken;
            return requestToken;
        }
    }
}
