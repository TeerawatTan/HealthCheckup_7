using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.AmedQuestionMasters
{
    public interface IAmedQuestionMasterRepository
    {
        Task<List<AmedQuestionMaster>> GetAmedQuestionMastersAsync();
        Task<AmedQuestionMaster> GetAmedQuestionMasterByIDAsync(int id);
        Task<List<AmedQuestionMapChoiceMaster>> GetAmedQuestionMasterMapChoicesAsync();
        Task<AmedQuestionMapChoiceMaster> GetAmedQuestionMasterMapChoicesByQuestionIDAsync(int questionNum);
        Task<string> CreateAmedQuestionMasterAsync(AmedQuestionMaster data);
        Task<string> UpdateAmedQuestionMasterAsync(AmedQuestionMaster data);
        Task<string> DeleteAmedQuestionMasterAsync(AmedQuestionMaster data);
    }
}