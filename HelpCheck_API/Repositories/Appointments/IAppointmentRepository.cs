using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpCheck_API.Dtos.Appointments;
using HelpCheck_API.Models;

namespace HelpCheck_API.Repositories.Appointments
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetAppointmentByIDAsync(int appointmentId);
        Task<Appointment> GetAppointmentByIDAndNotBookAsync(int appointmentId);
        Task<Appointment> GetReserveAppointmentByIDAsync(int appointmentId);
        Task<GetAppointmentDto> GetAppointmentByMemberIDAsync(int memberId);
        Task<int> GetCountAppointmentAsync(int id);
        Task<string> CreateAppointmentAsync(Appointment data);
        Task<int> UpdateAppointmentAsync(Appointment data);
        Task<int> GetLastQueueAppointmentAsync(int appointmentId);
        Task<int> GetMaximumBookedAppointmentByIDAsync(int id);
        Task<int> GetCountBookedAppointmentAsync(int id);
        Task<List<GetAppointmentDetailDto>> GetAppointmentsByDateAsync(DateTime date);
        Task<string> DeleteAppointmentAsync(Appointment data);
        Task<Appointment> GetAppointmentByAppointIDAndMemberIdAsync(int appointmentId, int memberId);
    }
}