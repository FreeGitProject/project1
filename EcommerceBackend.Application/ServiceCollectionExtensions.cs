using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceBackend.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

            // Register AutoMapper
            services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

            // Register FluentValidation
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }
    }
}
