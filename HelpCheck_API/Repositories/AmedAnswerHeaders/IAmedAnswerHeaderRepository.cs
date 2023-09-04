using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.AmedAnswerHeaders
{
    public interface IAmedAnswerHeaderRepository
    {
        Task<IEnumerable<AmedAnswerHeader>> GetAmedAnswerHeadersAsync();
        Task<AmedAnswerHeader> GetAmedAnswerHeaderByIDAsync(int id);
        Task<string> CreateAmedAnswerHeaderAsync(AmedAnswerHeader data);
        Task<string> UpdateAmedAnswerHeaderAsync(AmedAnswerHeader data);
        Task<string> DeleteAmedAnswerHeaderAsync(AmedAnswerHeader data);
    }
}