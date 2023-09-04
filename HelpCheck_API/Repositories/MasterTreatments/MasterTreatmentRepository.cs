using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.MasterTreatments
{
    public class MasterTreatmentRepository : IMasterTreatmentRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterTreatmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public MasterTreatment GetMasterTreatmentByID(int id)
        {
            if (id == 0)
            {
                return new MasterTreatment();
            }

            MasterTreatment data = _context.MasterTreatments.Where(w => w.ID == id)?.FirstOrDefault();

            return data;
        }

        public string GetMasterTreatmentNameByID(int id)
        {
            if (id == 0)
            {
                return "";
            }
            try
            {
                var data = _context.MasterTreatments.Where(w => w.ID == id).AsQueryable();
                if (data == null)
                {
                    return "";
                }

                MasterTreatment masterTreatment = data.FirstOrDefault();
                return masterTreatment.Name;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async Task<List<MasterTreatment>> GetMasterTreatmentsAsync()
        {
            return await _context.MasterTreatments.ToListAsync();
        }
        
        public async Task<MasterTreatment> GetMasterTreatmentByIDAsync(int id)
        {
            if (id == 0)
            {
                return new MasterTreatment();
            }

            MasterTreatment data = await _context.MasterTreatments.Where(w => w.ID == id)?.FirstOrDefaultAsync();

            return data;
        }
        
        public async Task<string> CreateMasterTreatmentAsync(MasterTreatment masterTreatment)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                await _context.MasterTreatments.AddAsync(masterTreatment);
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

        public async Task<string> UpdateMasterTreatmentAsync(MasterTreatment masterTreatment)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.MasterTreatments.Update(masterTreatment);
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

        public async Task<string> DeleteMasterTreatmentAsync(MasterTreatment masterTreatment)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.MasterTreatments.Remove(masterTreatment);
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

        public async Task<int> GetMasterTreatmentIDByNameAsync(string TreatmentName)
        {
            if (!string.IsNullOrWhiteSpace(TreatmentName))
            {
                var Treatment = await _context.MasterTreatments.Where(w => TreatmentName.Contains(w.Name))?.FirstOrDefaultAsync();
                if (Treatment != null)
                {
                    return Treatment.ID;
                }
            }

            return 0;
        }
    }
}
