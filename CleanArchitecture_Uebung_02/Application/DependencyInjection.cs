using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Application.Pipeline;
using Domain.Contracts;
using Application.Interfaces;
using Application.Common.Mappings;

namespace Application;

/// <summary>
/// Erweiterungsmethoden für DI-Registrierung der Application-Services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Mapster Mappings konfigurieren
        BookMappingConfig.ConfigureBookMappings();
        LoanMappingConfig.ConfigureLoanMappings();

        // CQRS + MediatR + FluentValidation
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly);
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(IUnitOfWork).Assembly);

        // Domain Services (für Uniqueness-Checks)
        services.AddScoped<IBookUniquenessChecker, BookUniquenessChecker>();

        return services;
    }
}
