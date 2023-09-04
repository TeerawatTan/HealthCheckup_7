using System.Collections.Generic;
using System.Threading.Tasks;
using HelpCheck_API.Models;

namespace HelpCheck_API.Repositories.AppointmentSettings
{
    public interface IAppointmentSettingRepository
    {
        Task<List<AppointmentSetting>> GetAppointmentSettingsAsync();
        Task<AppointmentSetting> GetAppointmentSettingByIDAsync(int id);
        Task<string> CreateAppointmentSettingAsync(AppointmentSetting data);
        Task<string> UpdateAppointmentSettingAsync(AppointmentSetting data);
        Task<string> DeleteAppointmentSettingAsync(AppointmentSetting data);
    }
}