using System.Threading.Tasks;
using HelpCheck_API.Data;
using System.Linq;
using System;
using System.Collections.Generic;
using HelpCheck_API.Dtos.QuestionAndChoices;
using Microsoft.EntityFrameworkCore;
using HelpCheck_API.Models;
using HelpCheck_API.Dtos.Answers;
using HelpCheck_API.Constants;
using HelpCheck_API.Dtos.Reports;

namespace HelpCheck_API.Repositories.QuestionAndChoices
{
    public class QuestionAndChoiceRepository : IQuestionAndChoiceRepository
    {
        private readonly ApplicationDbContext _context;
        public QuestionAndChoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetQuestionAndChoiceDto>> GetAllQuestionMappingChoiceAsync()
        {
            List<GetQuestionAndChoiceDto> list = new List<GetQuestionAndChoiceDto>();

            List<GetQuestionHeaderDto> questions = await (from q in _context.AmedQuestionMasters
                                                          join h in _context.AmedAnswerHeaders on q.QuestionNum equals h.QuestionNum
                                                          join c in _context.AmedChoiceMasters on h.ChoiceNum equals c.ChoiceNum
                                                          where (q.IsActive.HasValue && q.IsActive.Value) && (c.IsActive.HasValue && c.IsActive.Value)
                                                          orderby q.QuestionID
                                                          select new GetQuestionHeaderDto
                                                          {
                                                              QuestionID = q.QuestionID,
                                                              QuestionNum = q.QuestionNum,
                                                              QuestionName = q.QuestionName
                                                          }).Distinct().ToListAsync();
            if (questions != null && questions.Count > 0)
            {
                foreach (var item in questions)
                {
                    List<GetChoiceDto> choices = await (from c in _context.AmedChoiceMasters
                                                        join h in _context.AmedAnswerHeaders on c.ChoiceNum equals h.ChoiceNum
                                                        where h.QuestionNum == item.QuestionNum && (c.IsActive.HasValue && c.IsActive.Value)
                                                        orderby c.ChoiceID
                                                        select new GetChoiceDto
                                                        {
                                                            ChoiceID = c.ChoiceID,
                                                            ChoiceNum = c.ChoiceNum,
                                                            ChoiceName = c.ChoiceName,
                                                            QuestionNum = h.QuestionNum,
                                                            Score = c.Score
                                                        }).Distinct().OrderBy(o => o.ChoiceID).ToListAsync();
                    GetQuestionAndChoiceDto data = new GetQuestionAndChoiceDto()
                    {
                        QuestionID = item.QuestionID,
                        QuestionNum = item.QuestionNum,
                        QuestionName = item.QuestionName,
                        Choices = choices
                    };

                    list.Add(data);
                }
            }
            return list.OrderBy(o => o.QuestionID).ToList();
        }

        public async Task<GetQuestionAndChoiceDto> GetQuestionAndChoiceByQuestionNumberAsync(string questionNumber)
        {
            GetQuestionAndChoiceDto data = new GetQuestionAndChoiceDto();
            GetQuestionHeaderDto question = await (from q in _context.AmedQuestionMasters
                                                   join h in _context.AmedAnswerHeaders on q.QuestionNum equals h.QuestionNum
                                                   join c in _context.AmedChoiceMasters on h.ChoiceNum equals c.ChoiceNum
                                                   where q.QuestionNum == questionNumber
                                                   && (q.IsActive.HasValue && q.IsActive.Value) && (c.IsActive.HasValue && c.IsActive.Value)
                                                   orderby q.QuestionID
                                                   select new GetQuestionHeaderDto
                                                   {
                                                       QuestionID = q.QuestionID,
                                                       QuestionNum = q.QuestionNum,
                                                       QuestionName = q.QuestionName
                                                   }).FirstOrDefaultAsync();
            if (question != null)
            {
                List<GetChoiceDto> choices = await (from c in _context.AmedChoiceMasters
                                                    join h in _context.AmedAnswerHeaders on c.ChoiceNum equals h.ChoiceNum
                                                    where h.QuestionNum == question.QuestionNum
                                                    && (c.IsActive.HasValue && c.IsActive.Value)
                                                    orderby c.ChoiceID
                                                    select new GetChoiceDto
                                                    {
                                                        ChoiceID = c.ChoiceID,
                                                        ChoiceNum = c.ChoiceNum,
                                                        ChoiceName = c.ChoiceName,
                                                        QuestionNum = h.QuestionNum,
                                                        Score = c.Score
                                                    }).Distinct().ToListAsync();

                data = new GetQuestionAndChoiceDto()
                {
                    QuestionID = question.QuestionID,
                    QuestionNum = question.QuestionNum,
                    QuestionName = question.QuestionName,
                    Choices = choices
                };
            }
            return data;
        }

        public async Task<List<GetAnswerDto>> GetAnswerAsync(int userId, int year = 0)
        {
            var data = await (from d in _context.AmedAnswerDetails
                              join q in _context.AmedQuestionMasters on new { d.QuestionID, d.QuestionNum } equals new { q.QuestionID, q.QuestionNum } into lQ
                              from q in lQ.DefaultIfEmpty()
                              join c in _context.AmedChoiceMasters on new { d.ChoiceID, d.ChoiceNum } equals new { c.ChoiceID, c.ChoiceNum } into lC
                              from c in lC.DefaultIfEmpty()
                              where d.MemberID == userId && (q.IsActive.HasValue && q.IsActive.Value) && (c.IsActive.HasValue && c.IsActive.Value)
                              orderby d.QuestionID, d.ChoiceID
                              select new GetAnswerDto
                              {
                                  QuestionID = d.QuestionID,
                                  QuestionNum = d.QuestionNum,
                                  QuestionName = q.QuestionName,
                                  ChoiceID = d.ChoiceID,
                                  ChoiceNum = d.ChoiceNum,
                                  ChoiceName = c.ChoiceName,
                                  MemberID = d.MemberID,
                                  AsnwerKeyIn = d.AnswerKeyIn ?? string.Empty,
                                  CreatedDate = d.CreatedDate,
                                  Score = c.Score
                              }).ToListAsync();

            if (data is not null)
            {
                if (year > 0)
                {
                    data = data.Where(w => w.CreatedDate.HasValue && w.CreatedDate.Value.Year == year).ToList();
                }
            }

            return data;
        }

        public async Task<AmedAnswerDetail> GetAnswerByQuestIDAsync(int userId, int questionId)
        {
            return await (from d in _context.AmedAnswerDetails
                          join q in _context.AmedQuestionMasters on new { d.QuestionID, d.QuestionNum } equals new { q.QuestionID, q.QuestionNum } into lQ
                          from q in lQ.DefaultIfEmpty()
                          join c in _context.AmedChoiceMasters on new { d.ChoiceID, d.ChoiceNum } equals new { c.ChoiceID, c.ChoiceNum } into lC
                          from c in lC.DefaultIfEmpty()
                          where d.MemberID == userId && d.QuestionID == questionId && d.CreatedDate.Year == DateTime.Now.Year 
                          && (q.IsActive.HasValue && q.IsActive.Value) && (c.IsActive.HasValue && c.IsActive.Value)
                          select d)?.FirstOrDefaultAsync();
        }

        public async Task<List<GetAnswerAllUserDto>> GetAllAnswerAsync(int year)
        {
            var data = await (from d in _context.AmedAnswerDetails
                              join q in _context.AmedQuestionMasters on new { d.QuestionID, d.QuestionNum } equals new { q.QuestionID, q.QuestionNum } into lQ
                              from q in lQ.DefaultIfEmpty()
                              join c in _context.AmedChoiceMasters on new { d.ChoiceID, d.ChoiceNum } equals new { c.ChoiceID, c.ChoiceNum } into lC
                              from c in lC.DefaultIfEmpty()
                              join u in _context.UserEntities on d.MemberID equals u.ID
                              where (q.IsActive.HasValue && q.IsActive.Value) && (c.IsActive.HasValue && c.IsActive.Value)
                              orderby d.QuestionID, d.ChoiceID
                              select new GetAnswerAllUserDto
                              {
                                  QuestionName = q.QuestionName,
                                  ChoiceName = c.ChoiceName,
                                  AsnwerKeyIn = d.AnswerKeyIn ?? string.Empty,
                                  CreatedDate = d.CreatedDate,
                                  MemberName = d.CreatedBy,
                                  MemberHn = u.Hn,
                                  MemberIdCard = u.IDCard,
                                  Score = c.Score
                              }).ToListAsync();

            if (data is not null)
            {
                if (year > 0)
                {
                    data = data.Where(w => w.CreatedDate.HasValue && w.CreatedDate.Value.Year == year).ToList();
                }
            }

            return data;
        }

        public async Task<string> RemoveChoiceInQuestionNumberAsync(string questionNum)
        {
            var getQuestionHeader = await _context.AmedAnswerHeaders.Where(w => w.QuestionNum == questionNum).ToListAsync();
            if (getQuestionHeader is null || getQuestionHeader.Count == 0)
            {
                return Constant.STATUS_SUCCESS;
            }

            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.AmedAnswerHeaders.RemoveRange(getQuestionHeader);
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

        public async Task<int> CountQuestionAndChoiceAsync(int memberId)
        {
            int[] q1 = { 2, 4, 6, 8, 10, 12, 14, 16, 18, 19, 20, 21, 25, 29, 30, 31, 32 };
            int[] c1 = { 3, 4, 9, 11, 12, 16, 21 };

            return await (from a in _context.AmedAnswerDetails
                          where a.MemberID == memberId && q1.Contains(a.QuestionID) && c1.Contains(a.ChoiceID)
                          select a).CountAsync();
        }

        public async Task<List<ViewReportPsychiatristCheck>> GetViewReportDailyPsychiatristCheck(int questId)
        {
            return await _context.ViewReportPsychiatristChecks.Where(w => w.QuestionID == questId && w.CreateDate.Year == DateTime.Now.Year).ToListAsync();
        }

        public async Task<string> GetHasAnswerCongenitalDiseaseResultAsync(int memberId)
        {
            var data = await _context.AmedAnswerDetails.FirstOrDefaultAsync(w => w.MemberID == memberId && w.QuestionID == 40 && w.ChoiceID == 35);
            return data is not null && data.AnswerKeyIn is not null ? data.AnswerKeyIn : null;
        }

        public async Task<GetReportAllPatientDto> GetSmokingBehaviorAsync(int memberId)
        {
            GetReportAllPatientDto response = new GetReportAllPatientDto();

            var data = _context.AmedAnswerDetails.Where(w => w.MemberID == memberId).AsEnumerable();
            if (data is not null)
            {
                if (data.Any(s => (s.QuestionID == 21 && s.ChoiceID == 16) || (s.QuestionID == 23 && s.ChoiceID == 19)))
                {
                    response.SmokingBehavior = 1;
                }
                else if (data.Any(s => s.QuestionID == 23 && s.ChoiceID == 18))
                {
                    response.SmokingBehavior = 2;
                }
                else if (data.Any(s => s.QuestionID == 23))
                {
                    response.SmokingBehavior = 3;
                }
                else
                {
                    response.SmokingBehavior = 0;
                }

                if (data.Any(s => (s.QuestionID == 25 && s.ChoiceID == 21) || (s.QuestionID == 27 && s.ChoiceID == 28 || s.ChoiceID == 29)))
                {
                    response.AlcoholBehavior = 1;
                }
                else if (data.Any(s => s.QuestionID == 27 && s.ChoiceID == 30))
                {
                    response.AlcoholBehavior = 2;
                }
                else if (data.Any(s => s.QuestionID == 27))
                {
                    response.AlcoholBehavior = 3;
                }
                else
                {
                    response.AlcoholBehavior = 0;
                }

                if (data.Any(s => s.QuestionID == 37 && s.ChoiceID == 44))
                {
                    response.ExerciseBehavior = 1;
                }
                else if (data.Any(s => s.QuestionID == 37 && s.ChoiceID == 45))
                {
                    response.ExerciseBehavior = 2;
                }
                else if (data.Any(s => s.QuestionID == 37))
                {
                    response.ExerciseBehavior = 3;
                }
                else
                {
                    response.ExerciseBehavior = 0;
                }
            }

            return await Task.FromResult(response);
        }
    }
}