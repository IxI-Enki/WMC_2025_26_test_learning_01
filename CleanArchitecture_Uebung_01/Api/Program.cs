using Application;
using Infrastructure;
using Microsoft.OpenApi.Models;

namespace Api
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DB-Connection aus appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("Default") 
                ?? throw new ArgumentException("Connection string 'Default' not found");

            // Registriert Infrastructure (DbContext, Repositories, UoW)
            builder.Services.AddInfrastructure(connectionString);
            builder.Services.AddApplication();

            // Web API Basics
            builder.Services.AddControllers();

            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Event Management API",
                    Version = "v1",
                    Description = "API für Event-Verwaltung mit Clean Architecture"
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Management API v1");
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
