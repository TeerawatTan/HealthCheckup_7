using System.Threading.Tasks;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Doctors;

namespace HelpCheck_API.Services.Doctors
{
    public interface IDoctorService
    {
        Task<ResultResponse> GetDoctorCheckByMemberIDAsync(int memberId);
        Task<ResultResponse> CreateDoctorCheckAsync(AddDoctorCheckDto addDto);
        Task<ResultResponse> UpdateDoctorCheckByMemberIDAsync(EditDoctorCheckDto editDto);
    }
}