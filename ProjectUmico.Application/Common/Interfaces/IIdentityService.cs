using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Contracts.Authentication.v1;

namespace ProjectUmico.Application.Common.Interfaces;

public interface IIdentityService
{
    public Task<AuthenticationResult> CreateUserAsync(UserRegistrationRequest request);
    public Task<AuthenticationResult> AuthenticateUser(UserAuthenticationRequest request);
}