using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        
        // var encryptionKey = RSA.Create(Encoding.ASCII.GetBytes(settings.Secret));
        // var privateEncryptionKey = new RsaSecurityKey(encryptionKey) {KeyId = encryptionKid};
        // var publicEncryptionKey = new RsaSecurityKey(encryptionKey.ExportParameters(false)) {KeyId = encryptionKid};

        var tokenDescriptor = new SecurityTokenDescriptor
        {
           
            Subject = new ClaimsIdentity(claims),
            Audience = "https://localhost:7098",
            Expires = DateTime.Now.Add(settings.TokenLifeTime),
            SigningCredentials = 
                new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature),
            EncryptingCredentials = new EncryptingCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.Aes256KW,
                SecurityAlgorithms.Aes256CbcHmacSha512),
            
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
