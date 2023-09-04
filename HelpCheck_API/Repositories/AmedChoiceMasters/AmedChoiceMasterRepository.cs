using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Repositories.AmedChoiceMasters
{
    public class AmedChoiceMasterRepository : IAmedChoiceMasterRepository
    {
        private readonly ApplicationDbContext _context;

        public AmedChoiceMasterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAmedChoiceMasterAsync(AmedChoiceMaster data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.CreatedDate = DateTime.Now;
                await _context.AmedChoiceMasters.AddAsync(data);
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

        public async Task<string> DeleteAmedChoiceMasterAsync(AmedChoiceMaster data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.AmedChoiceMasters.Remove(data);
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

        public async Task<AmedChoiceMaster> GetAmedChoiceMasterByIDAsync(int id)
        {
            var datas = await GetAmedChoiceMastersAsync();

            if (datas is null || id <= 0)
            {
                return new AmedChoiceMaster();
            }

            AmedChoiceMaster data = datas.Where(w => w.ChoiceID == id)?.FirstOrDefault();
            return data;
        }

        public async Task<IEnumerable<AmedChoiceMaster>> GetAmedChoiceMastersAsync()
        {
            return await _context.AmedChoiceMasters.Where(w => w.IsActive.HasValue && w.IsActive.Value).ToListAsync();
        }

        public async Task<string> UpdateAmedChoiceMasterAsync(AmedChoiceMaster data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.UpdatedDate = DateTime.Now;
                _context.AmedChoiceMasters.Update(data);
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