using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectUmico.Application;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // automapper mappings
        // validators
        // mediatR Behaviours
        return services;
    }
}