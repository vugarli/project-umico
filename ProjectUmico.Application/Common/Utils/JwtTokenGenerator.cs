using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProjectUmico.Api.Common;
using ProjectUmico.Application.Contracts.Authentication.v1;

namespace ProjectUmico.Application.Common.Utils;

public static class JwtTokenGenerator
{
    public static string GenerateToken(JwtSettings settings,ICollection<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var key = Encoding.ASCII.GetBytes(settings.Secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            // Expires = settings.Expires,
            Audience = "https://localhost:7098",
            Expires = DateTime.Today.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
