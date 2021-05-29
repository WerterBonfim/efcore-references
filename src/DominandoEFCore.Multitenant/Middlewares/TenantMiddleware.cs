using System.Threading.Tasks;
using DominandoEFCore.Multitenant.Extensions;
using DominandoEFCore.Multitenant.Provider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DominandoEFCore.Multitenant.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var tenant = httpContext.RequestServices.GetRequiredService<TenantData>();
            tenant.TenantId = httpContext.GetTenantId();

            await _next(httpContext);
        }
    }
}