using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectUmico.Infrastructure.Persistance;

namespace ProjectUmico.Infrastructure;

public static class InfrastructureServicesExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration Configuration)
    {
        
        // register app interfaces

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(Configuration["DbConnect"]);
        });
        
        // entity config?
        // inject config to decide whether to use in mem db or sql one
        // services register
        return services;
    }
}