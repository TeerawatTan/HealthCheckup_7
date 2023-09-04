using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Physicals;

namespace HelpCheck_API.Repositories.PhysicalExaminationMasters
{
    public class PhysicalRepository : IPhysicalRepository
    {
        private readonly ApplicationDbContext _context;
        public PhysicalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PhysicalSet>> GetPhysicalDropdownListAsync()
        {
            return await _context.PhysicalSets.ToListAsync<PhysicalSet>();
        }

        public async Task<List<GetPhysicalExaminationMasterDto>> GetPhysicalExaminationMasterListByPhysicalSetIDAsync(int physicalSetId)
        {
            var data = await (from ps in _context.PhysicalSets
                              join pm in _context.PhysicalExaminationMasters on ps.PhysicalID equals pm.PhysicalID
                              join pc in _context.PhysicalChoices on pm.PhChoiceID equals pc.ID
                              where ps.ID == physicalSetId && pc.IsActive.HasValue
                              select new GetPhysicalExaminationMasterDto
                              {
                                  ID = pc.ID,
                                  DescriptTh = pc.DescriptTh,
                                  DescriptEn = pc.DescriptEn,
                                  UnitTh = pc.UnitTh,
                                  UnitEn = pc.UnitEn,
                                  Image = pc.Image,
                                  BgColor = pc.BgColor
                              }).ToListAsync<GetPhysicalExaminationMasterDto>();
            return data;
        }


        public async Task<List<GetPhysicalExaminationMasterDto>> GetPhysicalExaminationMasterListByPhysicalIDAsync(int physicalId)
        {
            var data = await (from ps in _context.PhysicalSets
                              join pm in _context.PhysicalExaminationMasters on ps.PhysicalID equals pm.PhysicalID
                              join pc in _context.PhysicalChoices on pm.PhChoiceID equals pc.ID
                              where pm.PhysicalID == physicalId && pc.IsActive.HasValue
                              select new GetPhysicalExaminationMasterDto
                              {
                                  ID = pc.ID,
                                  DescriptTh = pc.DescriptTh,
                                  DescriptEn = pc.DescriptEn,
                                  UnitTh = pc.UnitTh,
                                  UnitEn = pc.UnitEn,
                                  Image = pc.Image,
                                  BgColor = pc.BgColor
                              }).ToListAsync<GetPhysicalExaminationMasterDto>();
            return data;
        }

        public async Task<List<GetPhysicalExaminationMasterDto>> GetPhysicalAnswerListByMemberIDAsync(int memberId)
        {
            var maxDate = await _context.AnswerPhysicals.Where(w => w.MemberID == memberId).Select(s => s.CreateDate).OrderByDescending(s => s).FirstOrDefaultAsync();
            var data = await (from ap in _context.AnswerPhysicals
                              join ps in _context.PhysicalSets on ap.PhysicalSetID equals ps.ID
                              join pc in _context.PhysicalChoices on ap.PhysicalChoiceID equals pc.ID
                              where ap.MemberID == memberId && (ap.CreateDate != null && maxDate != null && ap.CreateDate.Value >= maxDate.Value.AddMinutes(-1) && ap.CreateDate.Value <= maxDate.Value.AddMinutes(1))
                              select new GetPhysicalExaminationMasterDto
                              {
                                  ID = pc.ID,
                                  DescriptTh = pc.DescriptTh,
                                  DescriptEn = pc.DescriptEn,
                                  UnitTh = pc.UnitTh,
                                  UnitEn = pc.UnitEn,
                                  Image = pc.Image,
                                  BgColor = pc.BgColor,
                                  Value = ap.AnsphyKeyIn,
                                  CreatedDate = ap.CreateDate
                              }).ToListAsync<GetPhysicalExaminationMasterDto>();
            return data;
        }
    }
}