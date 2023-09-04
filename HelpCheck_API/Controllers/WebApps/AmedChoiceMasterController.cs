using HelpCheck_API.Dtos.AmedChoiceMasters;
using HelpCheck_API.Services.AmedChoiceMasters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    [Authorize]
    public class AmedChoiceMasterController : BaseController
    {
        private readonly IAmedChoiceMasterService _amedChoiceMasterService;

        public AmedChoiceMasterController(IAmedChoiceMasterService amedChoiceMasterService)
        {
            _amedChoiceMasterService = amedChoiceMasterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAmedChoiceMasters()
        {
            var result = await _amedChoiceMasterService.GetAmedChoiceMasterAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetAmedChoiceMasterByID(int id)
        {
            var result = await _amedChoiceMasterService.GetAmedChoiceMasterByIDAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAmedChoiceMaster([FromBody] AddAmedChoiceMasterDto addAmedChoiceMaster)
        {
            addAmedChoiceMaster.AccessToken = GetAccessTokenFromHeader();
            var result = await _amedChoiceMasterService.CreateAmedChoiceMasterAsync(addAmedChoiceMaster);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAmedChoiceMasterByID(int id, [FromBody] EditAmedChoiceMasterDto editAmedChoiceMasterDto)
        {
            editAmedChoiceMasterDto.AccessToken = GetAccessTokenFromHeader();
            var result = await _amedChoiceMasterService.UpdateAmedChoiceMasterByIDAsync(editAmedChoiceMasterDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmedChoiceMasterByID(int id)
        {
            string accessToken = GetAccessTokenFromHeader();
            var result = await _amedChoiceMasterService.DeleteAmedChoiceMasterByIDAsync(id, accessToken);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}
