using AspNetCore.Lib.Extensions;
using AspNetCore.Lib.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.common
{
    public static class HtmlReponseExtension
    {
        public static object HtmlReponse(this Controller controller, string view = null,
            object model = null, Result result = null)
        {
            try
            {
                bool success;
                if (result == null)
                    success = true;
                else if (result != null && result.Success)
                    success = true;
                else
                    success = false;

                return new
                {
                    success,
                    error = result == null ? "" : $"Error {result?.Error?.Code}",
                    html = view == null ? "" : controller.RenderRazorViewToString(view, model)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }


    }
}
