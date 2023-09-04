using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.MasterJobTypes
{
    public class MasterJobTypeRepository : IMasterJobTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterJobTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public MasterJobType GetMasterJobTypeByID(int id)
        {
            if (id == 0)
            {
                return new MasterJobType();
            }

            MasterJobType data = _context.MasterJobTypes.Where(w => w.ID == id)?.FirstOrDefault();

            return data;
        }

        public string GetMasterJobTypeNameByID(int id)
        {
            if (id == 0)
            {
                return "";
            }
            try
            {
                var data = _context.MasterJobTypes.Where(w => w.ID == id).AsQueryable();
                if (data is null)
                {
                    return "";
                }

                MasterJobType masterJobType = data.FirstOrDefault();
                return masterJobType?.Name;
            }
            catch (Exception)
            {
                return "";
            }
            
        }

        public async Task<MasterJobType> GetMasterJobTypeByIDAsync(int id)
        {
            if (id == 0)
            {
                return new MasterJobType();
            }

            MasterJobType data = await _context.MasterJobTypes.Where(w => w.ID == id)?.FirstOrDefaultAsync();

            return data;
        }
        
        public async Task<List<MasterJobType>> GetMasterJobTypesAsync()
        {
            return await _context.MasterJobTypes.ToListAsync();
        }
        
        public async Task<string> CreateMasterJobTypeAsync(MasterJobType masterJobType)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                await _context.MasterJobTypes.AddAsync(masterJobType);
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

        public async Task<string> UpdateMasterJobTypeAsync(MasterJobType masterJobType)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.MasterJobTypes.Update(masterJobType);
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

        public async Task<string> DeleteMasterJobTypeAsync(MasterJobType masterJobType)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.MasterJobTypes.Remove(masterJobType);
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
