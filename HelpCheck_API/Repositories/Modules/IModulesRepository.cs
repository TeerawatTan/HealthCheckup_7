using System.Collections.Generic;
using System.Threading.Tasks;
using HelpCheck_API.Models;

namespace HelpCheck_API.Repositories.Modules
{
    public interface IModuleRepository
    {
        Task<List<Module>> GetModulesAsync();
        Module GetModuleByID(int id);
        (string, string) GetModuleNameByID(int id);
        Task<string> CreateModuleAsync(Module module);
        Task<string> UpdateModuleAsync(Module module);
        Task<string> DeleteModuleAsync(Module module);
    }
}