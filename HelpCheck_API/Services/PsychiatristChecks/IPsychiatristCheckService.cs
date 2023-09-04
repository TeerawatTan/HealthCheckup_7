using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Psychiatrists;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.PsychiatristChecks
{
    public interface IPsychiatristCheckService
    {
        Task<ResultResponse> GetPsychiatristCheckByMemberIDAsync(int memberId);
        Task<ResultResponse> CreatePsychiatristCheckAsync(AddPsychiatristCheckDto addDto);
        Task<ResultResponse> UpdatePsychiatristCheckByMemberIDAsync(EditPsychiatristCheckDto editDto);
    }
}
