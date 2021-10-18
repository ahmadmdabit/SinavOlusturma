using Common.Helpers;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
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

            var user = context.HttpContext.Items["User"] as UserEntity;
            var Unauthoried = user == null;
            if (!Unauthoried && Roles?.Length > 0)
            {
                Unauthoried = !Roles.Split(',').Select(x => x.Trim()).Any(x => x == user?.Role);
            }
            if (Unauthoried)
            {
                // not logged in
                context.Result = new JsonResult(new ApiResult<Object>(false, null, new UnauthorizedResult().StatusCode, "Unauthorized"))
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }

        private static bool SkipAuthorization(AuthorizationFilterContext context)
        {
            if (context == null) return false;
            return context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
        }
    }
}