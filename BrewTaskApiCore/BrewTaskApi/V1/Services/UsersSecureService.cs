using BrewTaskApi.Database.Services;
using BrewTaskApi.V1.Services.Extensions;

namespace BrewTaskApi.V1.Services
{

    /// <summary>
    /// user login
    /// </summary>
    [BusinessService]
    public class UsersSecureService(UsersService users, SecurePasswordService secure, JwtService jwtService)
    {

        /// <summary>
        /// try log in
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UsersSecureResponse?> TryLogInAsync(UserLoginRequest request)
        {
            if (await users.GetPasswordHashAsync(request.Email) is not { } user)
                return null;

            if (!secure.Verify(request.Password, user.PasswordHash))
                return null;

            var userResponse = await users.GetRequiredAsync(user.Id);
            var token = jwtService.GenerateJwtToken(userResponse.Email, userResponse.Id);

            return new(token, userResponse);
        }

        /// <summary>
        /// user login
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        public record UserLoginRequest(string Email, string Password);

        /// <summary>
        /// user response
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="User"></param>
        public record UsersSecureResponse(string Token, UsersService.UserResponse User);
    }
}
