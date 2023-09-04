using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Dtos.Appointments;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Repositories.Appointments
{
    public class AppointmentRepository : IAppointmentRepository
    {

        private readonly ApplicationDbContext _context;
        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> GetAppointmentByAppointIDAndMemberIdAsync(int appointmentId, int memberId)
        {
            return await _context.Appointments.Where(w => w.AppointmentID == appointmentId && w.MemberID == memberId && w.IsBooked)?.OrderBy(o => o.ID).FirstOrDefaultAsync();
        }

        public async Task<Appointment> GetAppointmentByIDAsync(int appointmentId)
        {
            return await _context.Appointments.Where(w => w.AppointmentID == appointmentId && w.IsBooked)?.OrderBy(o => o.ID).FirstOrDefaultAsync();
        }

        public async Task<Appointment> GetAppointmentByIDAndNotBookAsync(int appointmentId)
        {
            return await _context.Appointments.Where(w => w.AppointmentID == appointmentId && !w.IsBooked)?.OrderBy(o => o.ID).FirstOrDefaultAsync();
        }

        public async Task<Appointment> GetReserveAppointmentByIDAsync(int appointmentId)
        {
            return await _context.Appointments.Where(w => w.AppointmentID == appointmentId && !w.IsBooked && !w.IsReserve)?.OrderBy(o => o.ID).FirstOrDefaultAsync();
        }

        public async Task<string> CreateAppointmentAsync(Appointment data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.CreateDate = DateTime.Now;
                await _context.Appointments.AddAsync(data);
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

        public async Task<int> UpdateAppointmentAsync(Appointment data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.UpdateDate = DateTime.Now;
                _context.Appointments.Update(data);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return data.QueueNo;
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                return 0;
            }
        }

        private async Task<IList<GetAppointmentDto>> GetAllAppointmentByMemberIdAsync(int memberId)
        {
            return await (from a in _context.Appointments
                          join ast in _context.AppointmentSettings on a.AppointmentID equals ast.ID
                          join d in _context.UserEntities on a.DoctorID equals d.ID into ld
                          from d in ld.DefaultIfEmpty()
                          where a.MemberID == memberId && a.IsBooked
                          orderby a.ID descending
                          select new GetAppointmentDto
                          {
                              ID = a.ID,
                              MemberID = a.MemberID,
                              AppointmentID = a.AppointmentID,
                              AppointmentDate = ast.AppointmentDate.Date,
                              AppointmentDateTimeStart = ast.AppointmentDateTimeStart,
                              AppointmentDateTimeEnd = ast.AppointmentDateTimeEnd,
                              QueueNo = a.QueueNo,
                              DoctorID = a.DoctorID,
                              DoctorFullName = a.DoctorID == 0 ? "" : d.TitleName + " " + d.FirstName + " " + d.LastName,
                          }).AsQueryable().ToListAsync<GetAppointmentDto>();
        }

        public async Task<GetAppointmentDto> GetAppointmentByMemberIDAsync(int memberId)
        {
            GetAppointmentDto dto = new GetAppointmentDto();

            var data = await GetAllAppointmentByMemberIdAsync(memberId);
            if (data is not null && data.Count > 0)
            {
                dto = data.FirstOrDefault();
            }

            return dto;
        }

        public async Task<int> GetCountAppointmentAsync(int id)
        {
            return await _context.Appointments.Where(w => w.AppointmentID == id && w.IsReserve).CountAsync();
        }

        public async Task<int> GetLastQueueAppointmentAsync(int appointmentId)
        {
            var appointment = new Appointment();
            var data = await (from a in _context.Appointments
                              join ast in _context.AppointmentSettings on a.AppointmentID equals ast.ID
                              where a.AppointmentID == appointmentId && a.IsBooked
                              select a).ToListAsync();
            if (data.Count > 0)
            {
                appointment = data.OrderByDescending(o => o.QueueNo)?.FirstOrDefault();
            }

            return appointment != null ? appointment.QueueNo : 0;
        }

        public async Task<int> GetMaximumBookedAppointmentByIDAsync(int id)
        {
            var data = await (from a in _context.Appointments
                              join ast in _context.AppointmentSettings on a.AppointmentID equals ast.ID
                              where ast.ID == id
                              select ast).ToListAsync();
            int max = data.Count > 0 ? data.FirstOrDefault().MaximumPatient : 0;
            return max;
        }

        public async Task<int> GetCountBookedAppointmentAsync(int id)
        {
            return await _context.Appointments.Where(w => w.AppointmentID == id && w.IsBooked).CountAsync();
        }

        public async Task<List<GetAppointmentDetailDto>> GetAppointmentsByDateAsync(DateTime date)
        {
            var data = await (from a in _context.Appointments
                              join ast in _context.AppointmentSettings on a.AppointmentID equals ast.ID
                              join u in _context.UserEntities on a.MemberID equals u.ID
                              join t in _context.MasterTreatments on u.TreatmentID equals t.ID into lt
                              from t in lt.DefaultIfEmpty()
                              where ast.AppointmentDate == date
                              select new GetAppointmentDetailDto
                              {
                                  ID = a.ID,
                                  AppointmentID = a.ID,
                                  AppointmentDate = ast.AppointmentDate,
                                  AppointmentDateTimeStart = ast.AppointmentDateTimeStart.Value,
                                  AppointmentDateTimeEnd = ast.AppointmentDateTimeEnd.Value,
                                  MemberID = a.MemberID,
                                  MemberIdCard = u.IDCard,
                                  MemberHn = u.Hn,
                                  MemberName = (!string.IsNullOrWhiteSpace(u.TitleName) ? u.TitleName + " " : "") + u.FirstName + " " + u.LastName,
                                  TreatmentID = u.TreatmentID,
                                  TreatmentName = t.Name ?? string.Empty,
                                  Agency = u.Agency
                              }).ToListAsync();
            return data;
        }

        public async Task<string> DeleteAppointmentAsync(Appointment data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.Appointments.Remove(data);
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