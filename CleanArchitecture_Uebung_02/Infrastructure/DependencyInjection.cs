using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        string connectionString)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

        // Repositories und UoW (Scoped: pro HTTP-Request eine Instanz)
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // DataSeeder als Hosted Service (läuft beim Start automatisch)
        // WICHTIG: Dieser Service ist VOLLSTÄNDIG implementiert - beim echten Test wird das so sein!
        services.AddHostedService<StartupDataSeeder>();

        return services;
    }
}

