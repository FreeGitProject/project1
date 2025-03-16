namespace EcommerceBackend.Presentation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            // Register Controllers
            services.AddControllers();

            return services;
        }
    }
}
