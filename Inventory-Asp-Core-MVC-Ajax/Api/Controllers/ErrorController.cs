using Microsoft.AspNetCore.Mvc;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {

        public ErrorController()
        {
        }

        [Route("404")]
        public IActionResult PageNotFound() => View();


        [Route("500")]
        public IActionResult AppError() => View();

    }
}
