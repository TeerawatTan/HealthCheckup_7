using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.MasterOralHealths
{
    public class MasterOralHealthRepository : IMasterOralHealthRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterOralHealthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public MasterOralHealth GetMasterOralHealthByID(int id)
        {
            if (id == 0)
            {
                return new MasterOralHealth();
            }

            MasterOralHealth data = _context.MasterOralHealths.Where(w => w.ID == id)?.FirstOrDefault();

            return data;
        }

        public string GetMasterOralHealthCodeByID(int id)
        {
            if (id == 0)
            {
                return "";
            }
            try
            {
                var data = _context.MasterOralHealths.Where(w => w.ID == id).AsQueryable();
                if (data == null)
                {
                    return "";
                }

                MasterOralHealth masterOralHealth = data.FirstOrDefault();
                return masterOralHealth.Code;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string GetMasterOralHealthNameByID(int id)
        {
            if (id == 0)
            {
                return "";
            }
            try
            {
                var data = _context.MasterOralHealths.Where(w => w.ID == id).AsQueryable();
                if (data == null)
                {
                    return "";
                }

                MasterOralHealth masterOralHealth = data.FirstOrDefault();
                return masterOralHealth.Name;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string GetMasterOralHealthNameByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return "";
            }
            try
            {
                var data = _context.MasterOralHealths.Where(w => code.Contains(w.Code)).AsQueryable();
                if (data == null)
                {
                    return "";
                }

                MasterOralHealth masterOralHealth = data.FirstOrDefault();
                return masterOralHealth.Name;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async Task<MasterOralHealth> GetMasterOralHealthByIDAsync(int id)
        {
            if (id == 0)
            {
                return new MasterOralHealth();
            }

            MasterOralHealth data = await _context.MasterOralHealths.Where(w => w.ID == id)?.FirstOrDefaultAsync();

            return data;
        }

        public async Task<List<MasterOralHealth>> GetMasterOralHealthsAsync()
        {
            return await _context.MasterOralHealths.ToListAsync();
        }

        public async Task<string> CreateMasterOralHealthAsync(MasterOralHealth masterOralHealth)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                await _context.MasterOralHealths.AddAsync(masterOralHealth);
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

        public async Task<string> UpdateMasterOralHealthAsync(MasterOralHealth masterOralHealth)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.MasterOralHealths.Update(masterOralHealth);
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

        public async Task<string> DeleteMasterOralHealthAsync(MasterOralHealth masterOralHealth)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.MasterOralHealths.Remove(masterOralHealth);
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
