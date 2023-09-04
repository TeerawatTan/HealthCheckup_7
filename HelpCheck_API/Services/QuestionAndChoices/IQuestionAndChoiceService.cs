using System.Collections.Generic;
using System.Threading.Tasks;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.QuestionAndChoices;

namespace HelpCheck_API.Services.QuestionAndChoices
{
    public interface IQuestionAndChoiceService
    {
        Task<ResultResponse> GetAllQuestionAndChoiceAsync();
        Task<ResultResponse> GetQuestionAndChoiceByQuestionNumberAsync(string questionNum);
        Task<ResultResponse> CreateAnswerAsync(List<AddAnswerDto> addAnswerDtos, string accessToken);
        Task<ResultResponse> GetAnswerAsync(string accessToken, int year);
        Task<ResultResponse> GetAnswerByMemberIDAsync(int memberId);
        Task<ResultResponse> GetAllAnswerAsync(int year);
        Task<ResultResponse> EditQuestionAndChoiceAsync(string accessToken, string questionNumber, List<EditChoiceDto> editChoiceDtos);
        Task<ResultResponse> MappingChoiceWithQuestionAsync(string accessToken, string questionNumber, List<EditChoiceDto> editChoiceDtos);
    }
}