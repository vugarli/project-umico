using System.ComponentModel.Design;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using ProjectUmico.Application.Common.Behaviours;
using ProjectUmico.Application.Common.Identity;

namespace ProjectUmico.Application;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, SuperAdminRequirementHandler>();
        // automapper mappings
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        // validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // mediatR Behaviours
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        return services;
    }
}