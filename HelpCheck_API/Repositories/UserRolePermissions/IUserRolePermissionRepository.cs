using System.Collections.Generic;
using System.Threading.Tasks;
using HelpCheck_API.Models;

namespace HelpCheck_API.Repositories.UserRolePermissions
{
    public interface IUserRolePermissionRepository
    {
        Task<List<UserRolePermission>> GetUserRolePermissionsAsync();
        Task<List<UserRolePermission>> GetUserRolePermissionsByUserIDAsync(string userId);
    }
}