using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.PsychiatristChecks
{
    public class PsychiatristCheckRepository : IPsychiatristCheckRepository
    {
        private readonly ApplicationDbContext _context;

        public PsychiatristCheckRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreatePsychiatristCheckAsync(PsychiatristCheck data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.CreateDate = DateTime.Now;
                await _context.PsychiatristChecks.AddAsync(data);
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

        public async Task<PsychiatristCheck> GetPsychiatristCheckByMemberIDAsync(int memberId)
        {
            return await _context.PsychiatristChecks.Where(w => w.MemberID == memberId)?.OrderByDescending(o => o.ID)?.FirstOrDefaultAsync();
        }

        public async Task<string> UpdatePsychiatristCheckAsync(PsychiatristCheck data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.UpdateDate = DateTime.Now;
                _context.PsychiatristChecks.Update(data);
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
