using HelpCheck_API.Dtos.QuestionAndChoices;
using HelpCheck_API.Services.QuestionAndChoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    [Authorize]
    public class AmedQuestionHeaderController : BaseController
    {
        private readonly IQuestionAndChoiceService _questionAndChoiceService;

        public AmedQuestionHeaderController(IQuestionAndChoiceService questionAndChoiceService)
        {
            _questionAndChoiceService = questionAndChoiceService;
        }

        [HttpGet("{questionNumber}")]
        public async Task<ActionResult<List<GetQuestionAndChoiceDto>>> GetQuestionAndChoiceByQuestiosnNumber(string questionNumber)
        {
            var result = await _questionAndChoiceService.GetQuestionAndChoiceByQuestionNumberAsync(questionNumber);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost("MapQuestionAndChoice")]
        public async Task<ActionResult<GetQuestionAndChoiceDto>> MapChoiceByQuestionNum(string questionNumber, List<EditChoiceDto> editChoiceDtos)
        {
            string accessToken = GetAccessTokenFromHeader();

            if (string.IsNullOrWhiteSpace(questionNumber) || editChoiceDtos.Count == 0)
            {
                return BadRequest();
            }

            var result = await _questionAndChoiceService.EditQuestionAndChoiceAsync(accessToken, questionNumber, editChoiceDtos);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}
