using System.Threading.Tasks;
using HelpCheck_API.Dtos;

namespace HelpCheck_API.Services.MasterWorkPlaces
{
    public interface IMasterWorkPlaceService
    {
        Task<ResultResponse> GetMasterWorkPlacesAsync();
        Task<ResultResponse> CreateMasterWorkPlaceAsync(AddMasterDataDto addMasterDataDto);
        Task<ResultResponse> UpdateMasterWorkPlaceAsync(EditMasterDataDto editMasterDataDto);
        Task<ResultResponse> DeleteMasterWorkPlaceAsync(int id);
    }
}