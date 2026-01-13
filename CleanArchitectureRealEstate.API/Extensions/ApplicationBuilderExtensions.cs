using CleanArchitectureRealEstate.WebAPI.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CleanArchitectureRealEstate.WebAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
}
