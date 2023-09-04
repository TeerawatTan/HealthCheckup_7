using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Repositories.UserRolePermissions
{
    public class UserRolePermissionRepository : IUserRolePermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRolePermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserRolePermission>> GetUserRolePermissionsAsync()
        {
            return await _context.UserRolePermissions.ToListAsync();
        }
        
        public async Task<List<UserRolePermission>> GetUserRolePermissionsByUserIDAsync(string userId)
        {
            return await _context.UserRolePermissions.Where(w => w.UserID == userId).ToListAsync();
        }
    }
}