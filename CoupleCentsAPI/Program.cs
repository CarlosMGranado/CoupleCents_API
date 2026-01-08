using MediatR;
using FluentValidation;
using CoupleCentsAPI.Infrastructure.Data;
using CoupleCentsAPI.Common.Behaviors;
using CoupleCentsAPI.Domain.Services;
using CoupleCentsAPI.Infrastructure.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddScoped<IDbConnectionFactory, MySqlConnectionFactory>();
// Repositories(Data Access)
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

// Domain Services (Business Logic)
builder.Services.AddScoped<IExpenseService, ExpenseService>();

// MediatR
builder.Services.AddMediatR(typeof(Program));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Behaviors
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();