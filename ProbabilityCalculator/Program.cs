using FluentValidation;
using ProbabilityCalculator.API.Infrastructure;
using ProbabilityCalculator.API.Models;
using ProbabilityCalculator.API.Services;
using ProbabilityCalculator.API.Validators;
using System.IO.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICalculationService, CalculationService>();
builder.Services.AddScoped<ILoggingRepository, FileLoggingRepository>();
builder.Services.AddSingleton<IFileSystem, FileSystem>();
builder.Services.AddScoped<IValidator<Probability>, ProbabilityInputValidator>();

// Disabled for testing locally, we wouldn't do this in production ever
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        corsBuilder =>
        {
            corsBuilder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();

// Configure routing to use lowercase URLs.
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
