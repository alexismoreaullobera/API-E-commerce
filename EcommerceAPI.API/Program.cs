using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Infrastructure.Data;
using EcommerceAPI.Domain.Interfaces;
using EcommerceAPi.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using FluentValidation;
using EcommerceAPI.Application.Validators;
using EcommerceAPI.Application.Interfaces;
using EcommerceAPI.Application.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using EcommerceAPI.Application.Mappings;


var builder = WebApplication.CreateBuilder(args);

// ===== Configuration de la base de données =====
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configuration AutoMapper
builder.Services.AddAutoMapper(typeof(ProductProfile));

// Configuration FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();

//Configuation des services
builder.Services.AddScoped<IProductService, ProductService>();

// Configuration des repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// ===== Services API =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EcommerceAPI",
        Version = "v1",
        Description = "API E-commerce avec Clean Archi"
    });

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// ===== Configure the HTTP request pipeline. =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
