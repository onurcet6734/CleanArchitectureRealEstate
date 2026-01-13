using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Infrastructure.Persistence.Context;
using CleanArchitectureRealEstate.Infrastructure.Persistence.Repositories;
using CleanArchitectureRealEstate.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureRealEstate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            services.AddHttpContextAccessor();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFlatRepository, FlatRepository>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();


            return services;
        }
    }
}
