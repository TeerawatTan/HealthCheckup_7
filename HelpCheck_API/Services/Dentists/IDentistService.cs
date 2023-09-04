using System.Threading.Tasks;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Dentists;

namespace HelpCheck_API.Services.Dentists
{
    public interface IDentistService
    {
        Task<ResultResponse> GetDentistCheckByMemberIDAsync(int memberId);
        Task<ResultResponse> CreateDentistCheckAsync(AddDentistCheckDto addDto);
        Task<ResultResponse> UpdateDentistCheckByMemberIDAsync(EditDentistCheckDto editDto);
    }
}