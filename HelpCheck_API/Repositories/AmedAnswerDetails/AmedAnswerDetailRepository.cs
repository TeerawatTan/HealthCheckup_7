using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Repositories.AmedAnswerDetails
{
    public class AmedAnswerDetailRepository : IAmedAnswerDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public AmedAnswerDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAmedAnswerDetailAsync(AmedAnswerDetail data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.CreatedDate = DateTime.Now;
                await _context.AmedAnswerDetails.AddAsync(data);
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

        public async Task<string> DeleteAmedAnswerDetailAsync(AmedAnswerDetail data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.AmedAnswerDetails.Remove(data);
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

        public async Task<AmedAnswerDetail> GetAmedAnswerDetailByIDAsync(int id)
        {
            var datas = await GetAmedAnswerDetailsAsync();

            if (datas is null || id <= 0)
            {
                return new AmedAnswerDetail();
            }

            AmedAnswerDetail data = datas.Where(w => w.AnswerDetaiID == id)?.FirstOrDefault();
            return data;
        }

        public async Task<IEnumerable<AmedAnswerDetail>> GetAmedAnswerDetailsAsync()
        {
            return await _context.AmedAnswerDetails.ToListAsync();
        }

        public async Task<string> UpdateAmedAnswerDetailAsync(AmedAnswerDetail data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.UpdatedDate = DateTime.Now;
                _context.AmedAnswerDetails.Update(data);
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