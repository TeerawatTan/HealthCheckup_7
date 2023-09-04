using HelpCheck_API.Dtos.Psychiatrists;
using HelpCheck_API.Services.PsychiatristChecks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    [Authorize]
    public class PsychiatristController : BaseController
    {
        private readonly IPsychiatristCheckService _psychiatristCheckService;
        public PsychiatristController(IPsychiatristCheckService psychiatristCheckService)
        {
            _psychiatristCheckService = psychiatristCheckService;
        }

        [HttpGet("{memberId}")]
        public async Task<IActionResult> GetDoctorCheckByMemberID(int memberId)
        {
            var result = await _psychiatristCheckService.GetPsychiatristCheckByMemberIDAsync(memberId);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctorCheck([FromBody] AddPsychiatristCheckDto addDto)
        {
            addDto.AccessToken = GetAccessTokenFromHeader();

            if (addDto.MemberID <= 0)
            {
                return BadRequest("Member is not null");
            }

            var result = await _psychiatristCheckService.CreatePsychiatristCheckAsync(addDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{memberId}")]
        public async Task<IActionResult> UpdateDoctorCheck(int memberId, [FromBody] EditPsychiatristCheckDto editDto)
        {
            editDto.MemberID = memberId;
            editDto.AccessToken = GetAccessTokenFromHeader();
            var result = await _psychiatristCheckService.UpdatePsychiatristCheckByMemberIDAsync(editDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}
