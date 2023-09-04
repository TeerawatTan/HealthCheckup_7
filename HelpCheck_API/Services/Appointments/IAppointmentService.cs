using System;
using System.Threading.Tasks;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Appointments;

namespace HelpCheck_API.Services.Appointments
{
    public interface IAppointmentService
    {
        Task<ResultResponse> GetAllAppointmentSettingAsync(FilterAppointmentDto filterDto);
        Task<ResultResponse> GetAppointmentSettingByAccessTokenAsync(string accessToken);
        Task<ResultResponse> GetAppointmentSettingByIDAsync(int id);
        Task<ResultResponse> CreateAppointmentSettingAsync(AddAppointmentSettingDto addDto);
        Task<ResultResponse> UpdateAppointmentSettingAsync(EditAppointmentSettingDto editDto);
        Task<ResultResponse> DeleteAppointmentSettingAsync(int id);
        Task<ResultResponse> GetCountAppointmentSettingByIDAsync(int id);
        Task<ResultResponse> BookingAppointmentAsync(AddBookingAppointmentDto addDto);
        Task<ResultResponse> UnReserveAppointmentAsync(int id, string accessToken);
        Task<ResultResponse> GetAppointmentByDateAsync(DateTime date);
        Task<ResultResponse> CancelBookedByAppointmentIDAsync(int id, string token);

        Task<ResultResponse> GetAllAppointmentSettingMobileAsync(FilterAppointmentDto filterDto);
    }
}