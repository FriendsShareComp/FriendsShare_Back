using AccessData.Services;
using Aplication.Interfaces;
using Aplication.Services;
using Aplication.Utils;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var myApiKey = builder.Configuration["ApiKey"];
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(myApiKey)),
        RequireExpirationTime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.FromDays(7),
        LifetimeValidator = (notBefore, expires, token, validationParameters) =>
        {
            // Verificar si el token ha expirado
            return expires > DateTime.UtcNow;
        }

    };
});

builder.Services.AddSingleton(new JwtAuthManager(myApiKey));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese: 'bearer eyJhbGciOiJodHRwOi8vd3d3...'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Configurar AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configurar base de datos y servicios
builder.Services.Configure<UserStoreDatabaseSettings>(builder.Configuration.GetSection(key: "UserStoreDatabase"));
builder.Services.AddSingleton<UserService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserCommands, UserCommands>();
builder.Services.AddTransient<IAuthService, AuthServices>();

// Agregar puntos de acceso para la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Agregar autenticación y autorización al pipeline de solicitudes HTTP
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
