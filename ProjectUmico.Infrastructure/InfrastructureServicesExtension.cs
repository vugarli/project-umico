using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Infrastructure.Persistance;
using ProjectUmico.Infrastructure.Persistance.Interceptors;

namespace ProjectUmico.Infrastructure;

public static class InfrastructureServicesExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration Configuration, string env)
    {
        // register app interfaces
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();


        if (env is "Development")
        {
            services.AddDbContext<SqlLiteDbContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<SqlLiteDbContext>());
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        }


        // entity config?
        // inject config to decide whether to use in mem db or sql one
        // services register
        return services;
    }
}