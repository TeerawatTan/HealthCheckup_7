using System.Threading.Tasks;
using HelpCheck_API.Dtos;

namespace HelpCheck_API.Services.MasterJobTypes
{
    public interface IMasterJobTypeService
    {
        Task<ResultResponse> GetMasterJobTypesAsync();
        Task<ResultResponse> CreateMasterJobTypeAsync(AddMasterDataDto addMasterDataDto);
        Task<ResultResponse> UpdateMasterJobTypeAsync(EditMasterDataDto editMasterDataDto);
        Task<ResultResponse> DeleteMasterJobTypeAsync(int id);
    }
}