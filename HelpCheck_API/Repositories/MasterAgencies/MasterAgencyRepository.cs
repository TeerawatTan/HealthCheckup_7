using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.MasterAgencies
{
    public class MasterAgencyRepository : IMasterAgencyRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterAgencyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MasterAgency>> GetMasterAgenciesAsync()
        {
            return await _context.MasterAgencies.ToListAsync();
        }

        public MasterAgency GetMasterAgencyByID(int id)
        {
            if (id == 0)
            {
                return new MasterAgency();
            }

            MasterAgency data = _context.MasterAgencies.Where(w => w.ID == id)?.FirstOrDefault();

            return data;
        }

        public string GetMasterAgencyNameByID(int id)
        {
            if (id == 0)
            {
                return "";
            }
            try
            {
                var data = _context.MasterAgencies.Where(w => w.ID == id).AsQueryable();
                if (data == null)
                {
                    return "";
                }

                MasterAgency masterAgency = data.FirstOrDefault();
                return masterAgency.Name;
            }
            catch (Exception)
            {
                return "";
            }
            
        }

        public async Task<MasterAgency> GetMasterAgencyByIDAsync(int id)
        {
            if (id == 0)
            {
                return new MasterAgency();
            }

            MasterAgency data = await _context.MasterAgencies.Where(w => w.ID == id)?.FirstOrDefaultAsync();

            return data;
        }

        public async Task<string> CreateMasterAgencyAsync(MasterAgency masterAgency)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                await _context.MasterAgencies.AddAsync(masterAgency);
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

        public async Task<string> UpdateMasterAgencyAsync(MasterAgency masterAgency)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.MasterAgencies.Update(masterAgency);
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

        public async Task<string> DeleteMasterAgencyAsync(MasterAgency MasterAgency)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.MasterAgencies.Remove(MasterAgency);
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
