using Microsoft.OpenApi.Models;
using System.Reflection;

namespace TaskManagementSystem.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Task Management API",
                Version = "v1",
                Description = "API для управления задачами"
            });

            var assemblies = new[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(TaskManagementSystem.Application.DTOs.TaskDto).Assembly,
                typeof(TaskManagementSystem.Domain.Entities.TaskItem).Assembly
            };

            foreach (var assembly in assemblies.Distinct())
            {
                var xmlName = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlName);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            }
        });

        return services;
    }
}