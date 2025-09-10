using BrewTaskApi.Database.Contexts;
using BrewTaskApi.V1.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BrewTaskApi.V1.Services
{

    /// <summary>
    /// users
    /// </summary>
    /// <param name="context"></param>
    [BusinessService]
    public class UsersService(BrewTaskContext context)
    {

        /// <summary>
        /// get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserResponse?> GetAsync(int id)
        {
            var result = await context.Users
                .AsNoTracking()
                .Where(user => user.Id == id)
                .Select(user => new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.CreateAt,
                    AuthorTasksCount = user.AuthorTasks.Count(),
                    AssigneeTasksCount = user.AssigneeTasks.Count()
                })
                .FirstOrDefaultAsync();

            return result == null ? null : new UserResponse(
                result.Id,
                result.Username,
                result.Email,
                result.CreateAt,
                result.AuthorTasksCount,
                result.AssigneeTasksCount
            );
        }

        /// <summary>
        /// get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserResponse> GetRequiredAsync(int id)
        {
            var result = await context.Users
                .AsNoTracking()
                .Where(user => user.Id == id)
                .Select(user => new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.CreateAt,
                    AuthorTasksCount = user.AuthorTasks.Count(),
                    AssigneeTasksCount = user.AssigneeTasks.Count()
                })
                .FirstAsync();

            return new UserResponse(
                result.Id,
                result.Username,
                result.Email,
                result.CreateAt,
                result.AuthorTasksCount,
                result.AssigneeTasksCount
            );
        }


        /// <summary>
        /// get
        /// </summary>
        /// <returns></returns>
        public Task<UserSecureConfig?> GetPasswordHashAsync(string email)
            => context.Users
                .AsNoTracking()
                .Where(w => w.Email == email)
                .Select(s => new UserSecureConfig(s.Id, s.PasswordHash))
                .FirstOrDefaultAsync();

        /// <summary>
        /// user secure, not is response entity
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="PasswordHash"></param>
        public record UserSecureConfig(int Id, string PasswordHash);

        /// <summary>
        /// user response
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Username"></param>
        /// <param name="Email"></param>
        /// <param name="CreateAt"></param>
        /// <param name="AuthorTasksCount"></param>
        /// <param name="AssigneeTasksCount"></param>
        public record UserResponse(int Id, string Username, string Email, DateTime CreateAt, int AuthorTasksCount, int AssigneeTasksCount);
    }
}
