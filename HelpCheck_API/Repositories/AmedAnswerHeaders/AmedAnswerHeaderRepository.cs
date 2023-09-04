using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Repositories.AmedAnswerHeaders
{
    public class AmedAnswerHeaderRepository : IAmedAnswerHeaderRepository
    {
        private readonly ApplicationDbContext _context;

        public AmedAnswerHeaderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAmedAnswerHeaderAsync(AmedAnswerHeader data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                await _context.AmedAnswerHeaders.AddAsync(data);
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

        public async Task<string> DeleteAmedAnswerHeaderAsync(AmedAnswerHeader data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.AmedAnswerHeaders.Remove(data);
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

        public async Task<AmedAnswerHeader> GetAmedAnswerHeaderByIDAsync(int id)
        {
            var datas = await GetAmedAnswerHeadersAsync();

            if (datas is null || id <= 0)
            {
                return new AmedAnswerHeader();
            }

            AmedAnswerHeader data = datas.Where(w => w.ID == id)?.FirstOrDefault();
            return data;
        }

        public async Task<IEnumerable<AmedAnswerHeader>> GetAmedAnswerHeadersAsync()
        {
            return await _context.AmedAnswerHeaders.ToListAsync();
        }

        public async Task<string> UpdateAmedAnswerHeaderAsync(AmedAnswerHeader data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.UpdatedDate = DateTime.Now;
                _context.AmedAnswerHeaders.Update(data);
                await _context.SaveChangesAsync();

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