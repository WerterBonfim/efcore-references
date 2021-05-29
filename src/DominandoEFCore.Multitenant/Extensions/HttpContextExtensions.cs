using System;
using Microsoft.AspNetCore.Http;

namespace DominandoEFCore.Multitenant.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetTenantId(this HttpContext httpContext)
        {
            var tenant = httpContext.Request.Path.Value
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            return tenant[0];
        }
    }
}