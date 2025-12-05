using System.Text.Json;
using BackendProject.BLL.Services;
using BackendProject.DAL;
using BackendProject.DAL.Interfaces;
using BackendProject.DAL.Repositories;
using BackendProject.Validators;
using BackendProject.Config;
using Dapper;
using FluentValidation;
using Migrations;


var builder = WebApplication.CreateBuilder(args); 
DefaultTypeMap.MatchNamesWithUnderscores = true; 
builder.Services.AddScoped<UnitOfWork>(); 

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information); 

builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(nameof(DbSettings)));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IAuditLogOrderRepository, AuditOrderRepository>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.AddScoped<RabbitMqService>();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddScoped<ValidatorFactory>();
builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});


builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

Migrations.Program.Main([]); 

app.Run();