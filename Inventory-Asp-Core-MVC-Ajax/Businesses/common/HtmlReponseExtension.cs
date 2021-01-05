using AspNetCore.Lib.Extensions;
using AspNetCore.Lib.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Common
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


                string error;
                if (result == null)
                    error = "";
                else if (result?.Error.Data != null && result?.Error.Data.Length > 0)
                    error = $"Error ({result?.Error?.Code}) {result?.Error?.Data[0]}";
                else
                    error = $"Error ({result?.Error?.Code})";


                return new
                {
                    success,
                    error,
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
