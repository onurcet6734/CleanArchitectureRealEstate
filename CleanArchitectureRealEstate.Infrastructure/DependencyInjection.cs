using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Domain.Entities;
using CleanArchitectureRealEstate.Infrastructure.Persistence.Context;
using CleanArchitectureRealEstate.Infrastructure.Persistence.Repositories;
using CleanArchitectureRealEstate.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
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

            // 🔹 GEREKLİ: Identity altyapısı (migration’ın çalışması için ŞART)
            services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddHttpContextAccessor();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFlatRepository, FlatRepository>();
            services.AddScoped<IFlatImageRepository, FlatImageRepository>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddHttpClient<IEDevletService, EDevletService>();

            return services;
        }
    }
}
