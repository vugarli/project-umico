using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Infrastructure.Identity;
using ProjectUmico.Infrastructure.Persistance;
using ProjectUmico.Infrastructure.Persistance.Interceptors;

namespace ProjectUmico.Infrastructure;

public static class InfrastructureServicesExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration Configuration, string env)
    {
        services.AddScoped<IIdentityService,IdentityService>();

        // register app interfaces
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();


        if (env is not "Development")
        {
            services.AddDbContext<SqlLiteDbContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<SqlLiteDbContext>());
            
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<SqlLiteDbContext>();
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

        }


        // entity config?
        // inject config to decide whether to use in mem db or sql one
        // services register
        return services;
    }
}