using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.MasterWorkPlaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.MasterWorkPlaces
{
    public class MasterWorkPlaceRepository : IMasterWorkPlaceRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterWorkPlaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public MasterWorkPlace GetMasterWorkPlaceByID(int id)
        {
            if (id == 0)
            {
                return new MasterWorkPlace();
            }

            MasterWorkPlace data = _context.MasterWorkPlaces.Where(w => w.ID == id)?.FirstOrDefault();

            return data;
        }

        public string GetMasterWorkPlaceNameByID(int id)
        {
            if (id == 0)
            {
                return "";
            }
            try
            {
                var data = _context.MasterWorkPlaces.Where(w => w.ID == id).AsQueryable();
                if (data == null)
                {
                    return "";
                }

                MasterWorkPlace masterWorkPlace = data.FirstOrDefault();
                return masterWorkPlace.Name;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async Task<List<MasterWorkPlace>> GetMasterWorkPlacesAsync()
        {
            return await _context.MasterWorkPlaces.ToListAsync();
        }
        
        public async Task<MasterWorkPlace> GetMasterWorkPlaceByIDAsync(int id)
        {
            if (id == 0)
            {
                return new MasterWorkPlace();
            }

            MasterWorkPlace data = await _context.MasterWorkPlaces.Where(w => w.ID == id)?.FirstOrDefaultAsync();

            return data;
        }
        
        public async Task<string> CreateMasterWorkPlaceAsync(MasterWorkPlace masterWorkPlace)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                await _context.MasterWorkPlaces.AddAsync(masterWorkPlace);
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

        public async Task<string> UpdateMasterWorkPlaceAsync(MasterWorkPlace masterWorkPlace)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.MasterWorkPlaces.Update(masterWorkPlace);
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

        public async Task<string> DeleteMasterWorkPlaceAsync(MasterWorkPlace masterWorkPlace)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.MasterWorkPlaces.Remove(masterWorkPlace);
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
