using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Infrastructure.Data;
using EcommerceAPI.Application.Mappings;
using EcommerceAPI.Domain.Interfaces;
using EcommerceAPi.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// ===== Configuration de la base de données =====
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configuration AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configuration des repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// ===== Services API =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== Configure the HTTP request pipeline. =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
