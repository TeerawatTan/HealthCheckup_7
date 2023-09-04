using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Patients;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Patients
{
    public interface IPatientService
    {
        Task<ResultResponse> GetPhysicalByTokenAsync(string accessToken);
        Task<ResultResponse> GetPhysicalByMemberIDAsync(int memberId);
        Task<ResultResponse> CreatePhysicalPatientAsync(AddPhysicalDto addPhysicalDto);
        Task<ResultResponse> UpdatePhysicalPatientAsync(EditPhysicalDto editPhysicalDto);
        Task<ResultResponse> GetAppointmentDateByMemberIDAsync(int memberId);
        Task<ResultResponse> GetPatientAppointmentAsync(FilterPatientAppointmentDto filterPatientAppointmentDto);
        Task<ResultResponse> GetAllPatientAppointmentAsync(FilterPatientDto filterDto);

        Task<ResultResponse> GetXRayResultByIdCard(string idcard);
        Task<ResultResponse> GetBloodResultByHn(string hn);
        Task<ResultResponse> GetLabSmearByHn(string hn);
    }
}
