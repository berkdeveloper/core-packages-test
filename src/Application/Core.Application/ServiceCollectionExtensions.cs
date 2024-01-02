using Core.Application.Pipelines.Behaviors.Transaction;
using Core.Application.Pipelines.Behaviors.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreApplication(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionScopeBehavior<,>));
        return services;
    }
}
