using EcommerceBackend.Application.Common;
using EcommerceBackend.Application.Validators.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
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
            services.AddValidators();

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    List<string> errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(e => e.Value.Errors.Select(error => error.ErrorMessage))
                        .ToList();

                    var response = ApiResponse<object>.ValidationError(errors);
                    return new BadRequestObjectResult(response);
                };
            }).AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<LoginUserDtoValidator>();
            });

            return services;
        }
    }
}
