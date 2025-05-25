using FluentValidation.AspNetCore;
using FluentValidation;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Infrastructure;
using TaskManagementSystem.Application.Validators;
using TaskManagementSystem.API.Middleware;
using TaskManagementSystem.API.Extensions;
using Serilog;




Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateTaskRequestValidator>();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCustomSwagger();


builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build()
        .MigrateAndSeedDatabase();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();