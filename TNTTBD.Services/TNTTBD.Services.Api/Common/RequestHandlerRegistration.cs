using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Behaviors;

namespace TNTTBD.Services.Api.Common;

public static class RequestHandlerRegistration
{
    private const string QueriesNamespaceSuffix = ".Queries";

    /// <summary>
    /// Registers every <see cref="IRequestHandler{TRequest,TResponse}"/> and <see cref="IRequestHandler{TRequest}"/>
    /// in the assembly. Command handlers are wrapped in a transactional decorator; handlers for requests
    /// in a <c>*.Queries</c> namespace are read-only and registered without a transaction.
    /// </summary>
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<Type> handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false });

        foreach (Type handlerType in handlerTypes)
        {
            foreach (Type serviceType in handlerType.GetInterfaces().Where(i => i.IsGenericType))
            {
                Type definition = serviceType.GetGenericTypeDefinition();
                Type? decoratorDefinition =
                    definition == typeof(IRequestHandler<,>) ? typeof(TransactionalRequestHandler<,>)
                    : definition == typeof(IRequestHandler<>) ? typeof(TransactionalRequestHandler<>)
                    : null;

                if (decoratorDefinition is null)
                {
                    continue;
                }

                if (services.Any(d => d.ServiceType == serviceType))
                {
                    throw new InvalidOperationException($"Multiple handlers registered for {serviceType}.");
                }

                Type[] typeArguments = serviceType.GetGenericArguments();

                if (IsQuery(typeArguments[0]))
                {
                    services.AddTransient(serviceType, handlerType);
                    continue;
                }

                Type decoratorType = decoratorDefinition.MakeGenericType(typeArguments);

                services.TryAddTransient(handlerType);
                services.AddTransient(serviceType, sp => ActivatorUtilities.CreateInstance(
                    sp, decoratorType, sp.GetRequiredService(handlerType)));
            }
        }

        return services;
    }

    private static bool IsQuery(Type requestType)
        => requestType.Namespace?.EndsWith(QueriesNamespaceSuffix, StringComparison.Ordinal) == true;
}
