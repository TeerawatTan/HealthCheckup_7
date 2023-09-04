using HelpCheck_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.MasterOralHealths
{
    public interface IMasterOralHealthRepository
    {
        Task<List<MasterOralHealth>> GetMasterOralHealthsAsync();
        MasterOralHealth GetMasterOralHealthByID(int id);
        string GetMasterOralHealthCodeByID(int id);
        string GetMasterOralHealthNameByID(int id);
        string GetMasterOralHealthNameByCode(string code);
        Task<MasterOralHealth> GetMasterOralHealthByIDAsync(int id);
        Task<string> CreateMasterOralHealthAsync(MasterOralHealth masterOralHealth);
        Task<string> UpdateMasterOralHealthAsync(MasterOralHealth masterOralHealth);
        Task<string> DeleteMasterOralHealthAsync(MasterOralHealth masterOralHealth);
    }
}
