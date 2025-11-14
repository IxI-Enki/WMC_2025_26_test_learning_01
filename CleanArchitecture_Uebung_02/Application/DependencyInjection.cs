using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Application.Pipeline;
using Domain.Contracts;
using Application.Interfaces;

namespace Application;

/// <summary>
/// Erweiterungsmethoden für DI-Registrierung der Application-Services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // CQRS + MediatR + FluentValidation
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly);
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(IUnitOfWork).Assembly);

        // TODO: Registriere hier die Domain Services (z.B. IBookUniquenessChecker)
        // services.AddScoped<IBookUniquenessChecker, BookUniquenessChecker>();

        return services;
    }
}

