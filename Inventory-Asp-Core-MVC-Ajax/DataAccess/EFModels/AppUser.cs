using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName  { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string LastName{ get; set; }

        public Guid BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
