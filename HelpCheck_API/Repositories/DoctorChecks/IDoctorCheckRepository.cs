using System.Threading.Tasks;
using HelpCheck_API.Models;

namespace HelpCheck_API.Repositories.DoctorChecks
{
    public interface IDoctorCheckRepository
    {
        Task<DoctorCheck> GetDoctorCheckByMemberIDAsync(int memberId);
        Task<string> CreateDoctorCheckAsync(DoctorCheck data);
        Task<string> UpdateDoctorCheckAsync(DoctorCheck data);
    }
}