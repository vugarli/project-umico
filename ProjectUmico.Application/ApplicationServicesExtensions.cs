using System.ComponentModel.Design;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectUmico.Application;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // automapper mappings
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        // validators
        // mediatR Behaviours
        return services;
    }
}