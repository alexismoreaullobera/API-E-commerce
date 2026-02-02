using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Infrastructure.Data;
using EcommerceAPI.Application.Mappings;


var builder = WebApplication.CreateBuilder(args);

// ===== Configuration de la base de données =====
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

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
