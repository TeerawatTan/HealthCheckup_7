using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Repositories.AppointmentSettings
{
    public class AppointmentSettingRepository : IAppointmentSettingRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentSettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppointmentSetting>> GetAppointmentSettingsAsync()
        {
            return await _context.AppointmentSettings.ToListAsync<AppointmentSetting>();
        }

        public async Task<AppointmentSetting> GetAppointmentSettingByIDAsync(int id)
        {
            return await _context.AppointmentSettings.Where(w => w.ID == id)?.FirstOrDefaultAsync();
        }

        public async Task<string> CreateAppointmentSettingAsync(AppointmentSetting data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.CreateDate = DateTime.Now;
                await _context.AppointmentSettings.AddAsync(data);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return Constant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<string> UpdateAppointmentSettingAsync(AppointmentSetting data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.UpdateDate = DateTime.Now;
                _context.AppointmentSettings.Update(data);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();

                return Constant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<string> DeleteAppointmentSettingAsync(AppointmentSetting data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.AppointmentSettings.Remove(data);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();

                return Constant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return ex.Message;
            }
        }
    }
}