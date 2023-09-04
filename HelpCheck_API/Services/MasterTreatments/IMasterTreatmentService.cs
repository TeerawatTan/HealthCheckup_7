using System.Threading.Tasks;
using HelpCheck_API.Dtos;

namespace HelpCheck_API.Services.MasterTreatments
{
    public interface IMasterTreatmentService
    {
        Task<ResultResponse> GetMasterTreatmentsAsync();
        Task<ResultResponse> CreateMasterTreatmentAsync(AddMasterDataDto addMasterDataDto);
        Task<ResultResponse> UpdateMasterTreatmentAsync(EditMasterDataDto editMasterDataDto);
        Task<ResultResponse> DeleteMasterTreatmentAsync(int id);
    }
}