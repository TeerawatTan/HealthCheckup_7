using HelpCheck_API.Dtos.Answers;
using HelpCheck_API.Dtos.QuestionAndChoices;
using HelpCheck_API.Dtos.Reports;
using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.QuestionAndChoices
{
    public interface IQuestionAndChoiceRepository
    {
        Task<List<GetQuestionAndChoiceDto>> GetAllQuestionMappingChoiceAsync();
        Task<GetQuestionAndChoiceDto> GetQuestionAndChoiceByQuestionNumberAsync(string questionNumber);
        Task<List<GetAnswerDto>> GetAnswerAsync(int userId, int year = 0);
        Task<AmedAnswerDetail> GetAnswerByQuestIDAsync(int userId, int questionId);
        Task<List<GetAnswerAllUserDto>> GetAllAnswerAsync(int year);
        Task<string> RemoveChoiceInQuestionNumberAsync(string questionNum);
        Task<int> CountQuestionAndChoiceAsync(int memberId);
        Task<List<ViewReportPsychiatristCheck>> GetViewReportDailyPsychiatristCheck(int questId);
        Task<string> GetHasAnswerCongenitalDiseaseResultAsync(int memberId);
        Task<GetReportAllPatientDto> GetSmokingBehaviorAsync(int memberId);
    }
}