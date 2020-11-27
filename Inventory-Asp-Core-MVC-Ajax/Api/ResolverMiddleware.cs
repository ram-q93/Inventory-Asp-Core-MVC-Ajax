using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Inventory_Asp_Core_MVC_Ajax.Api
{
    public class ResolverMiddleware
    {
        private readonly RequestDelegate _next;

        public ResolverMiddleware(RequestDelegate next)
        {
            _next = next;
        }

    

        public async Task InvokeAsync(HttpContext context)
        {
            var s = context;
           


            await _next(context);
        }
    }
}
