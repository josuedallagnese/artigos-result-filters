using System.Globalization;
using System.Reflection;
using ResultFilters.Core.MediatR;
using FluentValidation;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CoreServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreMediator(
            this IServiceCollection services,
            Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.Add(new ServiceDescriptor(result.InterfaceType, result.ValidatorType, lifetime)));

            services.AddMediatR(config => config.Using<ValidationMediator>(), assembly);

            services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>), lifetime));

            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-BR");

            return services;
        }
    }
}
