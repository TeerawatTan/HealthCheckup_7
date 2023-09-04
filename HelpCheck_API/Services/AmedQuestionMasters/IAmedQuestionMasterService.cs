using System.Threading.Tasks;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.AmedQuestionMasters;

namespace HelpCheck_API.Services.AmedQuestionMasters
{
    public interface IAmedQuestionMasterService
    {
        Task<ResultResponse> GetAmedQuestionMasterMapChoicesAsync();
        Task<ResultResponse> GetAmedQuestionMasterMapChoicesByQuestionIDAsync(int id);
        Task<ResultResponse> CreateAmedQuestionMasterAsync(AddAmedQuestionMasterDto data);
        Task<ResultResponse> UpdateAmedQuestionMasterByIDAsync(EditAmedQuestionMasterDto data);
        Task<ResultResponse> DeleteAmedQuestionMasterByIDAsync(int id, string accessToken);
    }
}