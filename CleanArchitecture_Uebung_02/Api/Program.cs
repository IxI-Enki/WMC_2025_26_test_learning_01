using Application;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.OpenApi.Models;

namespace Api
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Prüfe ob Infrastructure übersprungen werden soll (für API-Tests!)
            var skipInfrastructure = builder.Configuration["SkipInfrastructure"]?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;

            var connectionString = builder.Configuration.GetConnectionString("Default") 
                ?? throw new ArgumentException("Connection string 'Default' not found");

            // DataSeeder Options konfigurieren
            builder.Services.Configure<StartupDataSeederOptions>(options =>
            {
                options.JsonPath = Path.Combine(AppContext.BaseDirectory, "library-seed-data.json");
            });

            // Conditional Registration: Skip Infrastructure in API-Tests
            if (!skipInfrastructure)
            {
                builder.Services.AddInfrastructure(connectionString);
            }
            builder.Services.AddApplication();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Library Management API",
                    Version = "v1",
                    Description = "API für Bibliotheksverwaltung mit Clean Architecture"
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management API v1");
                    c.RoutePrefix = "swagger";
                    c.DisplayRequestDuration();
                });
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
