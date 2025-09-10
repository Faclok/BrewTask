using BrewTaskApi.JWT;
using BrewTaskApi.V1.Services.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BrewTaskApi.V1.Services
{

    /// <summary>
    /// get jwt
    /// </summary>
    /// <param name="options"></param>
    [BusinessService]
    public class JwtService(AuthenticationOptions options)
    {

        /// <summary>
        /// Генерация нового JWT
        /// </summary>
        /// <param name="login">почта аккаунта</param>
        /// <param name="id"></param>
        /// <returns>новый JWT</returns>
        public string GenerateJwtToken(string login, int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([new Claim(ClaimTypes.Email, login), new Claim(ClaimTypes.NameIdentifier, id.ToString())]),
                Expires = DateTimeOffset.UtcNow.DateTime.AddMonths(2),
                Issuer = options.Issuer,
                Audience = options.Audience,
                SigningCredentials = new SigningCredentials(options.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
