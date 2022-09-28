using System.Runtime;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectUmico.Api.Common;
using ProjectUmico.Application;
using ProjectUmico.Application.Common.Identity;
using ProjectUmico.Infrastructure;
using ProjectUmico.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.EnvironmentName);


var jwtSettings = new JwtSettings();
builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.Audience = "https://localhost:7098";
        // options.Authority = "https://localhost:7098";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            
            TokenDecryptionKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            
            ValidateIssuer = false,
            ValidateActor = false,
            RequireExpirationTime = false,
            ValidateLifetime = true,
            RequireAudience = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization(authBuilder =>
{
    authBuilder.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    
    authBuilder.AddPolicy("RequiresSuperAdminRole", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new SuperAdminRequirement());
    });
});


builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    // if client tries to negotiate fot the media type that server doesn't serve give  HTTP 406 NotAccepted
    // https://code-maze.com/content-negotiation-web-api/ check for more
    options.ReturnHttpNotAcceptable = true;
}).AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo() {Description = "ProjectUmico", Title = "ProjectUmico"});
    
    var securityScheme = new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT" // Optional
    };

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            new string[] {}
        }
    };

    options.AddSecurityDefinition("bearerAuth", securityScheme);
    options.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});


var app = builder.Build();

ApplySeedingExtensions.SeedUserData(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();