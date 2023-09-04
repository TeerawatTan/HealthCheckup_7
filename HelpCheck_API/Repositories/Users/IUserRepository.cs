using HelpCheck_API.Dtos.Users;
using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.Users
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(FilterUserDto filterUserDto);
        Task<User> GetUserByUserIDAsync(string userId);
        Task<User> GetUserByAccessTokenAsync(string accessToken);
        Task<User> GetUserInfoByUserNameAsync(string userName);
        Task<bool> CheckUserExists(string username);
        Task<string> CreateUserAsync(User user);
        Task<string> UpdateUserAsync(User user);
        Task<string> DeleteUserAsync(User user);
        Task<User> GetUserByIDAsync(int id);
        Task<List<User>> GetAllUsers();
    }
}
