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
            //var generalDataService = (IGeneralDataService)context.RequestServices.GetService(typeof(IGeneralDataService));
            //if (context.User.Claims.Any())
            //{
            //    var claims = context.User.Claims;
            //    generalDataService.User = new User()
            //    {
            //        Id = new Guid(claims.FirstOrDefault(c => c.Type == "Id").Value),
            //        Name = claims.FirstOrDefault(c => c.Type == "Name").Value,
            //        Lastname = claims.FirstOrDefault(c => c.Type == "Lastname").Value,
            //        BusinessId = new Guid(claims.FirstOrDefault(c => c.Type == "BusinessId").Value),
            //        ParentBusinessId = string.IsNullOrWhiteSpace(claims.FirstOrDefault(c => c.Type == "ParentBusinessId").Value) ? (Guid?)null :
            //            new Guid(claims.FirstOrDefault(c => c.Type == "ParentBusinessId").Value),
            //        AgencyId = claims.Any(c => c.Type == "AgencyId" && !string.IsNullOrWhiteSpace(c.Value))
            //            ? (Guid?) new Guid(claims.FirstOrDefault(c => c.Type == "AgencyId").Value)
            //            : null,
            //        LocalSupplierId =
            //            claims.Any(c => c.Type == "LocalSupplierId" && !string.IsNullOrWhiteSpace(c.Value))
            //                ? (Guid?) new Guid(claims.FirstOrDefault(c => c.Type == "LocalSupplierId").Value)
            //                : null,
            //        RoleId = new Guid(claims.FirstOrDefault(c => c.Type == "RoleId").Value),
            //        IsB2B = claims.FirstOrDefault(c => c.Type == "IsB2B").Value == "True",
            //        Enabled = claims.FirstOrDefault(c => c.Type == "Enabled").Value == "True",
            //        Email = claims.FirstOrDefault(c => c.Type == "Email").Value,
            //        Mobile = claims.FirstOrDefault(c => c.Type == "Mobile").Value,
            //        Username = claims.FirstOrDefault(c => c.Type == "Username").Value,
            //        BusinessDomain = claims.FirstOrDefault(c => c.Type == "BusinessDomain").Value,
            //        AgencyDomain = claims.FirstOrDefault(c => c.Type == "AgencyDomain").Value,
            //        Permissions = claims.Any(c => c.Type == "PermissionsId" && !string.IsNullOrWhiteSpace(c.Value))
            //            ? claims.FirstOrDefault(c => c.Type == "PermissionsId").Value.Split(',').Select(s => Convert.ToInt32(s)).ToList()
            //            : null
            //    };
            //}


            await _next(context);
        }
    }
}
