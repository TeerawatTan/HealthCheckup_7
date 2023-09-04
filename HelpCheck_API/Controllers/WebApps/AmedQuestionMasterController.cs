using System.Threading.Tasks;
using HelpCheck_API.Dtos.AmedQuestionMasters;
using HelpCheck_API.Services.AmedQuestionMasters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpCheck_API.Controllers.WebApps
{
    [Authorize]
    public class AmedQuestionMasterController : BaseController
    {
        private readonly IAmedQuestionMasterService _amedQuestionMasterService;
        public AmedQuestionMasterController(IAmedQuestionMasterService amedQuestionMasterService)
        {
            _amedQuestionMasterService = amedQuestionMasterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAmedQuestionMasters()
        {
            var result = await _amedQuestionMasterService.GetAmedQuestionMasterMapChoicesAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetAmedQuestionMasterByID(int id)
        {
            var result = await _amedQuestionMasterService.GetAmedQuestionMasterMapChoicesByQuestionIDAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAmedQuestionMaster([FromBody] AddAmedQuestionMasterDto addAmedQuestionMaster)
        {
            addAmedQuestionMaster.AccessToken = GetAccessTokenFromHeader();
            var result = await _amedQuestionMasterService.CreateAmedQuestionMasterAsync(addAmedQuestionMaster);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAmedQuestionMasterByID(int id, [FromBody] EditAmedQuestionMasterDto editAmedQuestionMasterDto)
        {
            editAmedQuestionMasterDto.AccessToken = GetAccessTokenFromHeader();
            var result = await _amedQuestionMasterService.UpdateAmedQuestionMasterByIDAsync(editAmedQuestionMasterDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmedQuestionMasterByID(int id)
        {
            string accessToken = GetAccessTokenFromHeader();
            var result = await _amedQuestionMasterService.DeleteAmedQuestionMasterByIDAsync(id, accessToken);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}