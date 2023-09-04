using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Repositories.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.RolePermissions.ToListAsync();
        }

        public Role GetRoleByID(int id)
        {
            if (id == 0)
            {
                return new Role();
            }

            Role data = _context.RolePermissions.Where(w => w.ID == id)?.FirstOrDefault();

            return data;
        }

        public string GetRoleNameByID(int id)
        {
            if (id == 0)
            {
                return "";
            }

            Role data = _context.RolePermissions.Where(w => w.ID == id)?.FirstOrDefault();

            return data.RoleName;
        }

        public string GetRoleNameByRoleCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return "";
            }

            Role data = _context.RolePermissions.Where(w => w.RoleCode == code)?.FirstOrDefault();

            return data.RoleName;
        }
    }
}