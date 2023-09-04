using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Patients;
using HelpCheck_API.Dtos.Physicals;
using HelpCheck_API.Dtos.Reports;
using HelpCheck_API.Models;

namespace HelpCheck_API.Repositories.Patients
{
    public interface IPatientRepository
    {
        Task<List<AnswerPhysical>> GetLastAnswerPhysicalByMemberIDAsync(int memberId);
        Task<AnswerPhysical> GetAnswerPhysicalByIDAsync(int ansphyId, int memberId);
        Task<AnswerPhysical> GetLastAnswerPhysicalByMemberIDAndPhysicalSetIDAsync(int memberId, int phySetId);
        Task<string> CreatePhysicalPatientAsync(AnswerPhysical data);
        Task<string> UpdatePhysicalPatientAsync(AnswerPhysical data);
        Task<List<GetPatientAppointmentDto>> GetPatientAppointmentAsync(FilterPatientAppointmentDto filterPatientAppointmentDto);
        Task<List<GetAllPatientAppointmentDto>> GetAllPatientAppointmnetAsync(FilterPatientDto filterDto);
        Task<PageginationResultResponse<GetAllPatientAppointmentDto>> GetAllPatientAppointmnetAsync(PageRequestModel filter, CancellationToken cancellationToken);
        Task<PageginationResultResponse<GetAllPatientAppointmentDto>> GetReportAllPatientWithLastAppointmentAsync(DateTime dt, PageRequestModel filter, CancellationToken cancellationToken);
        Task<List<GetReportDentistCheckDto>> GetReportAllPatientWithDentistCheckAsync();
        Task<int> CountPhysicalWithPatientAsync(DateTime date);
        Task<List<GetReportDailyPsychiatristCheckDto>> GetPatientWithAnswerDetailAsync(int memberId);
        Task<PageginationResultResponse<GetAllPatientAppointmentDto>> GetAllPatientAppointmnetPaginationAsync(FilterPatientDto filterDto, PageRequestModel pageModel);
    }
}