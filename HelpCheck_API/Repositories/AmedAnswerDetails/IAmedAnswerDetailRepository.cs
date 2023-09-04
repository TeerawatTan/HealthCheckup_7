using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.AmedAnswerDetails
{
    public interface IAmedAnswerDetailRepository
    {
        Task<IEnumerable<AmedAnswerDetail>> GetAmedAnswerDetailsAsync();
        Task<AmedAnswerDetail> GetAmedAnswerDetailByIDAsync(int id);
        Task<string> CreateAmedAnswerDetailAsync(AmedAnswerDetail data);
        Task<string> UpdateAmedAnswerDetailAsync(AmedAnswerDetail data);
        Task<string> DeleteAmedAnswerDetailAsync(AmedAnswerDetail data);
    }
}