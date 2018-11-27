using Foodnetic.App.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Eventures.Web.Middlewares.MiddlewareExtensions
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseSeedRolesMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedRolesMiddleware>();
        }
    }
}