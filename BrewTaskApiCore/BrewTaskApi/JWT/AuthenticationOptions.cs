using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BrewTaskApi.JWT
{

    /// <summary>
    /// Настройки для JWT
    /// </summary>
    public class AuthenticationOptions
    {

        /// <summary>
        /// name in json
        /// </summary>
        public const string KEY_NAME_JWT = "JwtOptions";

        /// <summary>
        /// издатель токена
        /// </summary>
        public string Issuer { get; private set; } = string.Empty;

        /// <summary>
        /// потребитель токена
        /// </summary>
        public string Audience { get; private set; } = string.Empty;

        /// <summary>
        /// ключ для шифрации
        /// </summary>
        public string Key { get; private set; } = string.Empty;

        /// <inheritdoc/>
        public AuthenticationOptions()
        { /* use on mapper and default */ }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="key"></param>
        public AuthenticationOptions(string issuer, string audience, string key)
        {
            Issuer = issuer;
            Audience = audience;
            Key = key;
        }

        /// <summary>
        /// Генерация секретного ключа
        /// </summary>
        /// <returns></returns>
        public SymmetricSecurityKey GetSymmetricSecurityKey() 
            => new(Encoding.UTF8.GetBytes(Key));


        /// <summary>
        /// Генерация секретного ключа
        /// </summary>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key) 
            => new(Encoding.UTF8.GetBytes(key));

        /// <summary>
        /// Get options for configuration
        /// </summary>
        /// <param name="configuration">Objects contains all public properties</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static AuthenticationOptions GetOptions(IConfiguration configuration)
        {
            if (configuration.GetValue<string>($"{KEY_NAME_JWT}:{nameof(Issuer)}") is not { } issuer)
                throw new ArgumentException(nameof(Issuer));

            if (configuration.GetValue<string>($"{KEY_NAME_JWT}:{nameof(Audience)}") is not { } audience)
                throw new ArgumentException(nameof(Audience));

            if (configuration.GetValue<string>($"{KEY_NAME_JWT}:{nameof(Key)}") is not { } key)
                throw new ArgumentException(nameof(Key));

            return new(issuer, audience, key);
        }
    }
}
