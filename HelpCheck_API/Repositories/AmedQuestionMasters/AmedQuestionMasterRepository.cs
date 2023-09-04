using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Repositories.AmedQuestionMasters
{
    public class AmedQuestionMasterRepository : IAmedQuestionMasterRepository
    {
        private readonly ApplicationDbContext _context;

        public AmedQuestionMasterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AmedQuestionMaster>> GetAmedQuestionMastersAsync()
        {
            return await _context.AmedQuestionMasters.Where(w => w.IsActive.HasValue && w.IsActive.Value).ToListAsync();
        }

        public async Task<AmedQuestionMaster> GetAmedQuestionMasterByIDAsync(int id)
        {
            if (id == 0)
            {
                return new AmedQuestionMaster();
            }

            AmedQuestionMaster data = await _context.AmedQuestionMasters.Where(w => w.QuestionNum == id.ToString() && (w.IsActive.HasValue && w.IsActive.Value))?.FirstOrDefaultAsync();

            return data;
        }

        public async Task<string> CreateAmedQuestionMasterAsync(AmedQuestionMaster data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.CreatedDate = DateTime.Now;
                await _context.AmedQuestionMasters.AddAsync(data);
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

        public async Task<string> DeleteAmedQuestionMasterAsync(AmedQuestionMaster data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.AmedQuestionMasters.Remove(data);
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

        public async Task<AmedQuestionMapChoiceMaster> GetAmedQuestionMasterMapChoicesByQuestionIDAsync(int questionNum)
        {
            var data = await (from question in _context.AmedQuestionMasters
                              join choice in _context.AmedChoiceMasters on question.ChoiceID equals choice.ChoiceID
                              where question.QuestionNum == questionNum.ToString() 
                              && question.IsActive.HasValue && question.IsActive.Value && choice.IsActive.HasValue && choice.IsActive.Value
                              orderby question.QuestionID
                              select new AmedQuestionMapChoiceMaster
                              {
                                  QuestionID = question.QuestionID,
                                  QuestionNum = question.QuestionNum,
                                  QuestionName = question.QuestionName,
                                  QuestionPeriod = question.QuestionPeriod,
                                  ChoiceID = choice.ChoiceID,
                                  ChoiceName = choice.ChoiceName,
                                  CreatedBy = question.CreatedBy,
                                  CreatedDate = question.CreatedDate,
                                  Score = choice.Score
                              })?.FirstOrDefaultAsync();

            if (data is null)
            {
                return new AmedQuestionMapChoiceMaster();
            }

            return data;
        }

        public async Task<List<AmedQuestionMapChoiceMaster>> GetAmedQuestionMasterMapChoicesAsync()
        {
            var data = from question in _context.AmedQuestionMasters
                              join choice in _context.AmedChoiceMasters on question.ChoiceID equals choice.ChoiceID
                              where question.IsActive.HasValue && question.IsActive.Value && choice.IsActive.HasValue && choice.IsActive.Value
                              orderby question.QuestionID
                              select new AmedQuestionMapChoiceMaster
                              {
                                  QuestionID = question.QuestionID,
                                  QuestionNum = question.QuestionNum,
                                  QuestionName = question.QuestionName,
                                  QuestionPeriod = question.QuestionPeriod,
                                  ChoiceID = choice.ChoiceID,
                                  ChoiceName = choice.ChoiceName,
                                  CreatedBy = question.CreatedBy,
                                  CreatedDate = question.CreatedDate,
                                  Score = choice.Score
                              };
            return await data.ToListAsync();
        }

        public async Task<string> UpdateAmedQuestionMasterAsync(AmedQuestionMaster data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.UpdatedDate = DateTime.Now;
                _context.AmedQuestionMasters.Update(data);
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