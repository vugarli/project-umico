using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProjectUmico.Api.Common;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Common.Utils;
using ProjectUmico.Application.Contracts.Authentication.v1;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ProjectUmico.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
    }
    
    public async Task<AuthenticationResult> CreateUserAsync(UserRegistrationRequest registrationRequest)
    {
        var user = new ApplicationUser()
        {
            UserName = registrationRequest.Email,
            Email = registrationRequest.Email
        };
        
        var result = await _userManager.CreateAsync(user,registrationRequest.Password);

        var claims = await GetClaimsAsync(user);

        var token = JwtTokenGenerator.GenerateToken(_jwtSettings,claims);
        
        return AuthenticationResultExtensions.SuccessResponseFromToken(token);
    }

    public async Task<AuthenticationResult> AuthenticateUser(UserAuthenticationRequest request)
    {
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user is null) return AuthenticationResultExtensions.FailResponseForInvalidUserCredentials();

        var passCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

        if (passCorrect is false)
        {
            return AuthenticationResultExtensions.FailResponseForInvalidUserCredentials();
        }

        var claims = await GetClaimsAsync(user);
        
        var token = JwtTokenGenerator.GenerateToken(_jwtSettings,claims);

        return AuthenticationResultExtensions.SuccessResponseFromToken(token);
    }

    private async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);

        // Additional claims
        claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));
        claims.Add(new Claim(ClaimTypes.Email,user.Email));
        
        return claims;
    }
    
}