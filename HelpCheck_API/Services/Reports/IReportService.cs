using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Patients;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Reports
{
    public interface IReportService
    {
        Task<ResultResponse> GetReportAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<ResultResponse> GetReportDailyBccCheck();
        Task<ResultResponse> GetReportDailyPsychiatristCheck(DateTime dt, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<ResultResponse> GetReportDentistCheck();
        Task<ResultResponse> GetReportDailyHealthCheck(FilterPatientDto filter, int pageNumber, int pageSize);
    }
}
