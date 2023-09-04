using HelpCheck_API.Dtos.QuestionAndChoices;
using HelpCheck_API.Services.QuestionAndChoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    [Authorize]
    public class QuestionAndChoiceController : BaseController
    {
        private readonly IQuestionAndChoiceService _questionAndChoiceService;

        public QuestionAndChoiceController(IQuestionAndChoiceService questionAndChoiceService)
        {
            _questionAndChoiceService = questionAndChoiceService;
        }

        [HttpGet("MyAnswer/{id}")]
        public async Task<IActionResult> GetAnswer(int id)
        {
            var result = await _questionAndChoiceService.GetAnswerByMemberIDAsync(id);
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
        public async Task<IActionResult> GetQuestionAndChoiceByQuestionNumber(string num)
        {
            var result = await _questionAndChoiceService.GetQuestionAndChoiceByQuestionNumberAsync(num);
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

        [HttpPatch("Choice")]
        public async Task<ActionResult<GetQuestionAndChoiceDto>> EditChoiceByQuestionNum(string questionNumber, List<EditChoiceDto> editChoiceDtos)
        {
            string accessToken = GetAccessTokenFromHeader();
            
            if (editChoiceDtos.Count == 0)
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
