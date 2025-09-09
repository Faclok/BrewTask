using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BrewTaskApi.JWT
{

    /// <summary>
    /// jwt extensions
    /// </summary>
    public static class JWTExtensions
    {

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public static string? GetLogin(this HttpContext context)
            => context.Items[JWTMiddleware.CLAIMS_KEY]?.ToString();

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public static int? GetClientId(this HttpContext context)
            => int.TryParse(context.Items[JWTMiddleware.CLAIMS_KEY_CLIENT]?.ToString(), out var result) ? result : null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string? GetJwt(this HttpContext context)
            => context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        /// <summary>
        /// add jwt sheme on project
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddJWTokenBrewTask(this IServiceCollection services, IConfiguration configuration)
        {
            var options = AuthenticationOptions.GetOptions(configuration);
            services.AddAuthorization();
            services.AddSingleton(options);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = options.Issuer,
                        ValidateAudience = true,
                        ValidAudience = options.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = options.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            return services;
        }
    }
}
