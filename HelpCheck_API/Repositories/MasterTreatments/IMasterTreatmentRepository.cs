using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.MasterTreatments
{
    public interface IMasterTreatmentRepository
    {
        Task<List<MasterTreatment>> GetMasterTreatmentsAsync();
        MasterTreatment GetMasterTreatmentByID(int id);
        string GetMasterTreatmentNameByID(int id);
        Task<MasterTreatment> GetMasterTreatmentByIDAsync(int id);
        Task<string> CreateMasterTreatmentAsync(MasterTreatment masterTreatment);
        Task<string> UpdateMasterTreatmentAsync(MasterTreatment masterTreatment);
        Task<string> DeleteMasterTreatmentAsync(MasterTreatment masterTreatment);
        Task<int> GetMasterTreatmentIDByNameAsync(string TreatmentName);
    }
}
