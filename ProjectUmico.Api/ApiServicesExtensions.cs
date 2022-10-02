using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectUmico.Api.Common;
using ProjectUmico.Api.Services;
using ProjectUmico.Application.Common.Identity;
using ProjectUmico.Application.Common.Interfaces;

namespace ProjectUmico.Api;

public static class ApiServicesExtensions
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration Configuration)
    {
        // Jwt Settings
        var jwtSettings = new JwtSettings();
        Configuration.Bind(nameof(JwtSettings), jwtSettings);
        services.AddSingleton(jwtSettings);
        // User Service
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        //Authentication
        AddAuthentication(services, jwtSettings);
        // Authorization
        AddAuthorization(services);
        // HTTPCONTEXT ACCESSOR
        services.AddHttpContextAccessor();
        // Swagger
        AddSwagger(services);
        
    }

    private static void AddAuthentication(IServiceCollection services, JwtSettings jwtSettings)
    {
        services.AddAuthentication(options =>
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
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddAuthorization(authBuilder =>
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
    }

    private static void AddSwagger(IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        // https://stackoverflow.com/questions/71932980/what-is-addendpointsapiexplorer-in-asp-net-core-6
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() {Description = "ProjectUmico", Title = "ProjectUmico"});

            var securityScheme = new OpenApiSecurityScheme()
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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
                    new string[] { }
                }
            };

            options.AddSecurityDefinition("bearerAuth", securityScheme);
            options.AddSecurityRequirement(securityRequirement);
        });
    }
}