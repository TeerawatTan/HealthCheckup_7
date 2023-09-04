using System.Threading.Tasks;
using HelpCheck_API.Dtos;

namespace HelpCheck_API.Services.MasterAgencies
{
    public interface IMasterAgencyService
    {
        Task<ResultResponse> GetMasterAgenciesAsync();
        Task<ResultResponse> CreateMasterAgencyAsync(AddMasterDataDto addMasterDataDto);
        Task<ResultResponse> UpdateMasterAgencyAsync(EditMasterDataDto editMasterDataDto);
        Task<ResultResponse> DeleteMasterAgencyAsync(int id);
    }
}