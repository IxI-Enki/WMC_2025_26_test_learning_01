using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Domain.Contracts;
using Application.Interfaces.Repositories;
using Application.Interfaces;

namespace Api.Tests.Utilities;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Skip Infrastructure Layer (SQL Server) - Use InMemory Database instead
        builder.UseSetting("SkipInfrastructure", "true");

        builder.ConfigureServices(services =>
        {
            // Remove previously registered DbContext (if any)
            var toRemove = services.Where(d => 
                d.ServiceType == typeof(DbContextOptions<AppDbContext>) || 
                d.ServiceType == typeof(AppDbContext)
            ).ToList();
            foreach (var d in toRemove) services.Remove(d);

            // Register InMemory Database for API Tests
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("ApiTestsDb"));

            // Register repositories & UoW (mimic Infrastructure.AddInfrastructure, but without SQL / seeder)
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookUniquenessChecker, BookUniquenessChecker>();

            // Add Application layer MediatR + Validators if not already (idempotent)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly));
            services.AddValidatorsFromAssembly(typeof(IUnitOfWork).Assembly);

            // Build provider & ensure db created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
