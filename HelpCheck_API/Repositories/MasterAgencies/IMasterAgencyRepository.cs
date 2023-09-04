using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.MasterAgencies
{
    public interface IMasterAgencyRepository
    {
        Task<List<MasterAgency>> GetMasterAgenciesAsync();
        MasterAgency GetMasterAgencyByID(int id);
        string GetMasterAgencyNameByID(int id);
        Task<MasterAgency> GetMasterAgencyByIDAsync(int id);
        Task<string> CreateMasterAgencyAsync(MasterAgency masterAgency);
        Task<string> UpdateMasterAgencyAsync(MasterAgency masterAgency);
        Task<string> DeleteMasterAgencyAsync(MasterAgency MasterAgency);
    }
}
