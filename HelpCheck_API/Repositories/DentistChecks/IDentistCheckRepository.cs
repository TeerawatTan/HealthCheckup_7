using System.Collections.Generic;
using System.Threading.Tasks;
using HelpCheck_API.Models;

namespace HelpCheck_API.Repositories.DentistChecks
{
    public interface IDentistCheckRepository
    {
        Task<DentistCheck> GetDentistCheckByMemberIDAsync(int memberId);
        Task<DentistCheck> CreateDentistCheckAsync(DentistCheck data);
        Task<string> UpdateDentistCheckAsync(DentistCheck data);

        // Dentist check oral health
        Task<IEnumerable<DentistCheckOralHealth>> GetDentistCheckOralHealthsAsync();
        Task<IEnumerable<DentistCheckOralHealth>> GetDentistCheckByDentistIDAsync(int dentistId);
        Task<DentistCheckOralHealth> GetDentistCheckOralHealthAsync(int oralId, int dentistcheckId);
        Task<string> CreateDentistCheckOralHealthsAsync(List<DentistCheckOralHealth> list);
        Task<string> UpdateDentistCheckOralHealthAsync(DentistCheckOralHealth data);
        Task<string> DeleteAllDentistCheckOralHealthAsync(int dentistcheckId);
    }
}