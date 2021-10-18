using Common.Helpers;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
        /// </summary>
        public string Roles { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (SkipAuthorization(context)) return;

            var token = context.HttpContext.Request.Cookies["Token"];
            //var role = Encoding.UTF8.GetString(Convert.FromBase64String(context.HttpContext.Request.Cookies["Role"]));
            var role = Encoding.UTF8.GetString(StringToByteArray(context.HttpContext.Request.Cookies["Role"]));
            var Unauthoried = token == null;
            if (!Unauthoried && Roles?.Length > 0)
            {
                Unauthoried = !Roles.Split(',').Select(x => x.Trim()).Any(x => x == role);
            }
            if (Unauthoried)
            {
                // not logged in
                //context.Result = new JsonResult(new ApiResult<Object>(false, null, new UnauthorizedResult().StatusCode, "Unauthorized"))
                //{
                //    StatusCode = StatusCodes.Status401Unauthorized
                //};
                context.HttpContext.Response.Redirect("/Auth/Login");
            }
        }

        private static bool SkipAuthorization(AuthorizationFilterContext context)
        {
            if (context == null) return false;
            return context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
        }

        static byte[] StringToByteArray(String hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                return new byte[] { };
            }
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}