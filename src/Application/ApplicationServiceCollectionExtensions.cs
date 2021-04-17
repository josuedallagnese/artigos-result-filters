namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddCoreMediator(typeof(ApplicationServiceCollectionExtensions).Assembly);

            return services;
        }
    }
}
