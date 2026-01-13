using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureRealEstate.WebAPI.Filters;

namespace CleanArchitectureRealEstate.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}
