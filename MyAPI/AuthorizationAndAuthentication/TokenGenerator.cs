using Microsoft.IdentityModel.Tokens;
using MyAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyAPI.AuthorizationAndAuthentication
{
    public class TokenGenerator
    {
        public readonly TokenConfiguration _configuration;

        public TokenGenerator(TokenConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim("mod", "Web III in .NET"),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // JWT's header "alg"

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_configuration.ExpirationTimeInHours),
                signingCredentials: credentials);
                       
            return tokenHandler.WriteToken(jwtToken);
        }
    }
}