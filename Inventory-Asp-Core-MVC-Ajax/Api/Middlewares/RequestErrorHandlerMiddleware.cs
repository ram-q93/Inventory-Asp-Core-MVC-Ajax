using AspNetCore.Lib.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Middlewares
{
    public class RequestErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //try
            //{
                await _next(context);
                if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    context.Request.Path = "/error/404";
                    await _next(context);
                }
            //}
            //catch (Exception e)
            //{
            //    context.Request.Path = "/error/500";
            //    ((ILogger)context.RequestServices.GetService(typeof(ILogger))).Exception(e);
            //    var s =context.Features.Get<IExceptionHandlerFeature>();
            //    s = null;
            //    await _next(context);
            //}

        }
    }
}
