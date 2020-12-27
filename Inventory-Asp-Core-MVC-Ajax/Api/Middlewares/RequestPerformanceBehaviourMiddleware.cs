using AspNetCore.Lib.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Middlewares
{
    public class RequestPerformanceBehaviourMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Stopwatch _timer;

        public RequestPerformanceBehaviourMiddleware(RequestDelegate next)
        {
            _next = next;
            _timer = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _timer.Start();
            var s = new Stopwatch();
            s.Start();
            await _next(context);

            s.Stop();

            _timer.Stop();

            var _logger = (ILogger)context.RequestServices.GetService(typeof(ILogger));
            var _currentGeneralData = (IGeneralData)context.RequestServices.GetService(typeof(IGeneralData));

            var controllerActionDescriptor = context.GetEndpoint()?.Metadata
               .GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor?.ControllerName;
            var actionName = controllerActionDescriptor?.ActionName;
            _logger.Warn(s.ElapsedMilliseconds.ToString());
            if (_timer.ElapsedMilliseconds > 5000)
            {
                _logger.Warn($"Long Running Request:   ({_timer.ElapsedMilliseconds} milliseconds)" +
                    $" Controller:({controllerName})" +
                    $" Action:({actionName})" +
                    $" {_currentGeneralData?.UserData?.Email}");
            }
        }
    }
}
