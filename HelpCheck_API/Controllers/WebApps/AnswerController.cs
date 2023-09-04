using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Dtos.Answers;
using HelpCheck_API.Services.QuestionAndChoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpCheck_API.Controllers.WebApps
{
    [Authorize]
    public class AnswerController : BaseController
    {
        private readonly IQuestionAndChoiceService _questionAndChoiceService;

        public AnswerController(IQuestionAndChoiceService questionAndChoiceService)
        {
            _questionAndChoiceService = questionAndChoiceService;
        }

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadReport(int year)
        {
            try
            {
                string token = GetAccessTokenFromHeader();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var result = await _questionAndChoiceService.GetAllAnswerAsync(year);
                    if (result.IsSuccess)
                    {
                        return Ok(result.Data);
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}