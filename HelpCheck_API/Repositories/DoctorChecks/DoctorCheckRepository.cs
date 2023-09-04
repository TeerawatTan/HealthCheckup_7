using System;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Repositories.DoctorChecks
{
    public class DoctorCheckRepository : IDoctorCheckRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorCheckRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorCheck> GetDoctorCheckByMemberIDAsync(int memberId)
        {
            return await _context.DoctorChecks.Where(w => w.MemberID == memberId)?.OrderByDescending(o => o.ID).FirstOrDefaultAsync();
        }

        public async Task<string> CreateDoctorCheckAsync(DoctorCheck data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.CreateDate = DateTime.Now;
                await _context.DoctorChecks.AddAsync(data);
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

        public async Task<string> UpdateDoctorCheckAsync(DoctorCheck data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.UpdateDate = DateTime.Now;
                _context.DoctorChecks.Update(data);
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