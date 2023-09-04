using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.DentistChecks
{
    public class DentistCheckRepository : IDentistCheckRepository
    {
        private readonly ApplicationDbContext _context;

        public DentistCheckRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DentistCheck> GetDentistCheckByMemberIDAsync(int memberId)
        {
            return await _context.DentistChecks.Where(w => w.MemberID == memberId)?.OrderByDescending(o => o.ID).FirstOrDefaultAsync();
        }

        public async Task<DentistCheck> CreateDentistCheckAsync(DentistCheck data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.CreateDate = DateTime.Now;
                await _context.DentistChecks.AddAsync(data);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return data;
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                return data;
            }
        }

        public async Task<string> UpdateDentistCheckAsync(DentistCheck data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.UpdateDate = DateTime.Now;
                _context.DentistChecks.Update(data);
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

        public async Task<string> CreateDentistCheckOralHealthsAsync(List<DentistCheckOralHealth> list)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                await _context.DentistCheckOralHealths.AddRangeAsync(list);
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

        public async Task<IEnumerable<DentistCheckOralHealth>> GetDentistCheckOralHealthsAsync()
        {
            return await _context.DentistCheckOralHealths.ToListAsync<DentistCheckOralHealth>();
        }

        public async Task<IEnumerable<DentistCheckOralHealth>> GetDentistCheckByDentistIDAsync(int dentistId)
        {
            return await _context.DentistCheckOralHealths.Where(w => w.DentistCheckID == dentistId).ToListAsync<DentistCheckOralHealth>();
        }

        public async Task<DentistCheckOralHealth> GetDentistCheckOralHealthAsync(int oralId, int dentistcheckId)
        {
            return await _context.DentistCheckOralHealths.Where(w => w.OralID == oralId && w.DentistCheckID == dentistcheckId).FirstOrDefaultAsync<DentistCheckOralHealth>();
        }

        public async Task<string> UpdateDentistCheckOralHealthAsync(DentistCheckOralHealth data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.DentistCheckOralHealths.Update(data);
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

        public async Task<string> DeleteAllDentistCheckOralHealthAsync(int dentistcheckId)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var list = await _context.DentistCheckOralHealths.Where(w => w.DentistCheckID == dentistcheckId).ToListAsync();
                _context.DentistCheckOralHealths.RemoveRange(list);
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