using System.Threading.Tasks;
using HelpCheck_API.Dtos;

namespace HelpCheck_API.Services.MasterOralHealths
{
    public interface IMasterOralHealthService
    {
        Task<ResultResponse> GetMasterOralHealthsAsync();
        Task<ResultResponse> CreateMasterOralHealthAsync(AddMasterDataCodeAndNameDto addMasterDataDto);
        Task<ResultResponse> UpdateMasterOralHealthAsync(EditMasterCodeAndNameDto editMasterDataDto);
        Task<ResultResponse> DeleteMasterOralHealthAsync(int id);
    }
}