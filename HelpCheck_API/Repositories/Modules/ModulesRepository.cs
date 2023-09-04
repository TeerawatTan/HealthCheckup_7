using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Repositories.Modules
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext _context;

        public ModuleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Module>> GetModulesAsync()
        {
            return await _context.Modules.ToListAsync();
        }

        public Module GetModuleByID(int id)
        {
            if (id == 0)
            {
                return new Module();
            }

            Module data = _context.Modules.Where(w => w.ID == id)?.FirstOrDefault();

            return data;
        }

        public (string, string) GetModuleNameByID(int id)
        {
            if (id == 0)
            {
                return ("", "");
            }

            Module data = _context.Modules.Where(w => w.ID == id)?.FirstOrDefault();

            return (data.ModuleNameTh, data.ModuleNameEn);
        }
        
        public async Task<string> CreateModuleAsync(Module module)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                await _context.Modules.AddAsync(module);
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

        public async Task<string> UpdateModuleAsync(Module module)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.Modules.Update(module);
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

        public async Task<string> DeleteModuleAsync(Module module)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.Modules.Remove(module);
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