using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface ICategoryBiz
    {


        Result<object> ListName();
    }
}
