using HelpCheck_API.Dtos.QuestionAndChoices;
using HelpCheck_API.Services.QuestionAndChoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.MobileApps
{
    [Authorize]
    public class QuestionAndChoiceController : BaseController
    {
        private readonly IQuestionAndChoiceService _questionAndChoiceService;

        public QuestionAndChoiceController(IQuestionAndChoiceService questionAndChoiceService)
        {
            _questionAndChoiceService = questionAndChoiceService;
        }

        [HttpGet("MyAnswer/{year}")]
        public async Task<IActionResult> GetAnswer(int year)
        {
            string accessToken = GetAccessTokenFromHeader();
            var result = await _questionAndChoiceService.GetAnswerAsync(accessToken, year);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetQuestionAndChoice()
        {
            var result = await _questionAndChoiceService.GetAllQuestionAndChoiceAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("{questionNumber}")]
        public async Task<IActionResult> GetQuestionAndChoiceByQuestionNumber(string questionNumber)
        {
            var result = await _questionAndChoiceService.GetQuestionAndChoiceByQuestionNumberAsync(questionNumber);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost("Answer")]
        public async Task<IActionResult> CreateAnswer([FromBody] List<AddAnswerDto> addAnswerDtos)
        {
            string accessToken = GetAccessTokenFromHeader();
            var result = await _questionAndChoiceService.CreateAnswerAsync(addAnswerDtos, accessToken);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}
