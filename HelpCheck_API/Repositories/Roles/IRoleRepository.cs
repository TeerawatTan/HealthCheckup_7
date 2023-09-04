using System.Collections.Generic;
using System.Threading.Tasks;
using HelpCheck_API.Models;

namespace HelpCheck_API.Repositories.Roles
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRolesAsync();
        Role GetRoleByID(int id);

        string GetRoleNameByID(int id);
        string GetRoleNameByRoleCode(string code);
    }
}