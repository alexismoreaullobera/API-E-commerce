using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Infrastructure.Data;
using EcommerceAPI.Domain.Interfaces;
using FluentValidation.AspNetCore;
using FluentValidation;
using EcommerceAPI.Application.Validators;
using EcommerceAPI.Application.Interfaces;
using EcommerceAPI.Application.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using EcommerceAPI.Application.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EcommerceAPI.Infrastructure.Repositories;


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

// ===== Configuration des services d'authentification =====
string jwtSecretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new Exception("JWT Secret Key is not configured.");
string jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new Exception("JWT Issuer is not configured.");
string jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new Exception("JWT Audience is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey!))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez votre token JWT dans le format : Bearer {votre_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ===== Configure the HTTP request pipeline. =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
