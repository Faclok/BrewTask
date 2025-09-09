using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BrewTaskApi.JWT
{

    /// <summary>
    /// Промежуточный слой JWT
    /// </summary>
    /// <param name="next"></param>
    /// <param name="options"></param>
    public class JWTMiddleware(RequestDelegate next, AuthenticationOptions options)
    {

        /// <summary>
        /// Claims key
        /// </summary>
        public const string CLAIMS_KEY = "UserLogin";

        /// <summary>
        /// Claims key client
        /// </summary>
        public const string CLAIMS_KEY_CLIENT = "UserLoginId";

        private const string NULL_JAVASCRIPT = "null";

        /// <summary>
        /// Выполнение
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachAccountToContext(context, token);

            await next(context);
        }

        private async Task AttachAccountToContext(HttpContext context, string token)
        {
            if (token == NULL_JAVASCRIPT || string.IsNullOrWhiteSpace(token))
                return;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var resultToken = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = options.GetSymmetricSecurityKey(),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = options.Issuer,
                    ValidAudience = options.Audience
                });

                if (!resultToken.IsValid)
                    return;

                if (resultToken.SecurityToken is not JwtSecurityToken jwtToken)
                    return;


                if (context.User.Identity is not ClaimsIdentity claimsIdentity)
                    return;

                var accountLogin = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
                var accountId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (accountLogin is null || accountId is null)
                    return;

                context.Items[CLAIMS_KEY] = accountLogin;
                context.Items[CLAIMS_KEY_CLIENT] = accountId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
