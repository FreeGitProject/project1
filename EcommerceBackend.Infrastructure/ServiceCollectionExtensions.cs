using EcommerceBackend.Domain.Categories.Interfaces;
using EcommerceBackend.Domain.Common;
using EcommerceBackend.Domain.Interfaces;
using EcommerceBackend.Infrastructure.Data.Repositories;
using EcommerceBackend.Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackend.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            // Register Identity Services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            // Register Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            return services;
        }
    }
}
