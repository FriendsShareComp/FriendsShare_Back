using Domain.Models;
using Aplication.Services;
using Aplication.Interfaces;
using AccessData.Services;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Aplication.Utils;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();


builder.Services.AddControllers();


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

var key = "83kZz7QOdv9Sj3SqT1gS0sjTPqmGDqo8XVXzNDLL";
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        RequireExpirationTime = true,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddSingleton(new JwtAuthManager(key));
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add services to the container.
builder.Services.Configure<UserStoreDatabaseSettings>(
builder.Configuration.GetSection(key: "UserStoreDatabase"));
builder.Services.AddSingleton<UserService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserCommands, UserCommands>();
builder.Services.AddTransient<IAuthService, AuthServices>();

//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



// Configure the HTTP request pipeline.
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
