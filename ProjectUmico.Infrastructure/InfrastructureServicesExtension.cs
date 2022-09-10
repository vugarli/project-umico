using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Infrastructure.Persistance;
using ProjectUmico.Infrastructure.Persistance.Interceptors;

namespace ProjectUmico.Infrastructure;

public static class InfrastructureServicesExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration Configuration)
    {
        
        // register app interfaces
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(Configuration["DbConnect"]);
        });
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        // entity config?
        // inject config to decide whether to use in mem db or sql one
        // services register
        return services;
    }
}