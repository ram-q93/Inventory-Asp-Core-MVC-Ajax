using Microsoft.AspNetCore.Identity;
using System;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels
{
    public class User : IdentityUser
    {
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
