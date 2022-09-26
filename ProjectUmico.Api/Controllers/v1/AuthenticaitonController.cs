using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Contracts.Authentication.v1;

namespace ProjectUmico.Api.Controllers.v1;

[AllowAnonymous]
public class AuthenticaitonController : ApiControllerBasev1
{
    private readonly IIdentityService _identityService;

    public AuthenticaitonController(IIdentityService identityService)
    {
        this._identityService = identityService;
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest registerRequest)
    {
        var result = await _identityService.CreateUserAsync(registerRequest);
        return Ok(result);
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] UserAuthenticationRequest authenticationRequest)
    {
        var result = await _identityService.AuthenticateUser(authenticationRequest);
        return Ok(result);
    }

    [HttpPost("/claims")]
    public IActionResult GiveClaims()
    {
        if (User is not null)
        {
            var claims = User.Claims.Select(c=>new {c.Value,c.Type});

            return Ok(claims);
        }

        return BadRequest("Not authenticated");
    }
}