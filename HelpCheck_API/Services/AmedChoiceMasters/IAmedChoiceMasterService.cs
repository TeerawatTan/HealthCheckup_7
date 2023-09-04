using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.AmedChoiceMasters;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.AmedChoiceMasters
{
    public interface IAmedChoiceMasterService
    {
        Task<ResultResponse> GetAmedChoiceMasterAsync();
        Task<ResultResponse> GetAmedChoiceMasterByIDAsync(int id);
        Task<ResultResponse> CreateAmedChoiceMasterAsync(AddAmedChoiceMasterDto data);
        Task<ResultResponse> UpdateAmedChoiceMasterByIDAsync(EditAmedChoiceMasterDto data);
        Task<ResultResponse> DeleteAmedChoiceMasterByIDAsync(int id, string accessToken);
    }
}
