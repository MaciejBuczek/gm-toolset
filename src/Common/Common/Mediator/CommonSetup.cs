using Common.Mediator.Pipelines;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Mediator
{
    public static class CommonSetup
    {
        public static IServiceCollection AddRequestHandlers(this IServiceCollection services, Assembly assembly)
        {
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly)
        {
            services.AddValidatorsFromAssembly(
                assembly,
                includeInternalTypes: true);

            return services;
        }

        public static IServiceCollection DecorateRequestWithLogging(this IServiceCollection services)
        {
            if (services.HasRegisteredRequestHandlers())
            {
                services.Decorate(
                typeof(IRequestHandler<,>),
                typeof(LoggingRequestHandler<,>));
            }           

            return services;
        }

        public static IServiceCollection DecorateRequestWithValidation(this IServiceCollection services)
        {
            if (services.HasRegisteredRequestHandlers())
            {
                services.Decorate(
                typeof(IRequestHandler<,>),
                typeof(ValidationRequestHandler<,>));
            }

            return services;
        }

        public static IServiceCollection DecorateRequestWithEventHandling(this IServiceCollection services)
        {
            if (services.HasRegisteredRequestHandlers())
            {
                services.Decorate(
                typeof(IRequestHandler<,>),
                typeof(DomainEventsRequestHandler<,>));
                services.Decorate(
                    typeof(IRequestHandler<,>),
                    typeof(IntegrationEventsRequestHandler<,>));
            }

            return services;
        }

        private static bool HasRegisteredRequestHandlers(this IServiceCollection services)
        {
            return services.Any(s =>
                s.ServiceType.IsGenericType &&
                s.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
        }
    }
}
