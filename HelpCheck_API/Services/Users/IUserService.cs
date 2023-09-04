using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Users;
using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Users
{
    public interface IUserService
    {
        Task<ResultResponse> GetUsersAsync(FilterUserDto filterUserDto);
        Task<ResultResponse> GetUserProfileByAccessTokenAsync(string accessToken);
        Task<ResultResponse> RegisterAsync(AddUserDto addUserDto, bool? isMobile);
        Task<ResultResponse> UpdateUserInfoAsync(EditUserDto editUserDto);
        Task<ResultResponse> UpdateUserByIDAsync(EditUserDto editUserDto);
        Task<ResultResponse> DeleteUserByIDAsync(string userId);
        Task<ResultResponse> UpdateUserNotActiveAsync(string userId);
        Task<ResultResponse> GetUserProfileByIDAsync(int id);
        Task<ResultResponse> GetUserByOrtherServiceAsync(string idCard);
        Task<List<User>> GetAllUsers();
        Task<ResultResponse> PatchUserAsync(User user);
    }
}
