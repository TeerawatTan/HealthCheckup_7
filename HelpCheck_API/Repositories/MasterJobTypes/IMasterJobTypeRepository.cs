using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.MasterJobTypes
{
    public interface IMasterJobTypeRepository
    {
        Task<List<MasterJobType>> GetMasterJobTypesAsync();
        MasterJobType GetMasterJobTypeByID(int id);
        string GetMasterJobTypeNameByID(int id);
        Task<MasterJobType> GetMasterJobTypeByIDAsync(int id);
        Task<string> CreateMasterJobTypeAsync(MasterJobType masterJobType);
        Task<string> UpdateMasterJobTypeAsync(MasterJobType masterJobType);
        Task<string> DeleteMasterJobTypeAsync(MasterJobType masterJobType);
    }
}
