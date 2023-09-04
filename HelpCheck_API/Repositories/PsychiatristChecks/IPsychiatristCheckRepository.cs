using HelpCheck_API.Models;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.PsychiatristChecks
{
    public interface IPsychiatristCheckRepository
    {
        Task<PsychiatristCheck> GetPsychiatristCheckByMemberIDAsync(int memberId);
        Task<string> CreatePsychiatristCheckAsync(PsychiatristCheck data);
        Task<string> UpdatePsychiatristCheckAsync(PsychiatristCheck data);
    }
}
