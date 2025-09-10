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
        public Task<UserResponse?> GetAsync(int id)
            => context.Users
                .AsNoTracking()
                .Select(user => new UserResponse(user.Id, user.Username, user.Email, user.CreateAt, user.AuthorTasks.Count(), user.AssigneeTasks.Count()))
                .FirstOrDefaultAsync(a => a.Id == id);

        /// <summary>
        /// get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<UserResponse> GetRequiredAsync(int id)
            => context.Users
                .AsNoTracking()
                .Select(user => new UserResponse(user.Id, user.Username, user.Email, user.CreateAt, user.AuthorTasks.Count(), user.AssigneeTasks.Count()))
                .FirstAsync(a => a.Id == id);


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
