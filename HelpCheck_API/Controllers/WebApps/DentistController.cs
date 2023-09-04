using HelpCheck_API.Dtos.Dentists;
using HelpCheck_API.Services.Dentists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    [Authorize]
    public class DentistController : BaseController
    {
        private readonly IDentistService _dentistService;

        public DentistController(IDentistService dentistService)
        {
            _dentistService = dentistService;
        }

        [HttpGet("{memberId}")]
        public async Task<ActionResult<GetDentistCheckDto>> GetDentistCheckByMemberID(int memberId)
        {
            var result = await _dentistService.GetDentistCheckByMemberIDAsync(memberId);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDentistCheck([FromBody] AddDentistCheckDto addDto)
        {
            addDto.AccessToken = GetAccessTokenFromHeader();
            var result = await _dentistService.CreateDentistCheckAsync(addDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{memberId}")]
        public async Task<IActionResult> UpdateDentistCheck(int memberId, [FromBody] EditDentistCheckDto editDto)
        {
            editDto.MemberID = memberId;
            editDto.AccessToken = GetAccessTokenFromHeader();
            var result = await _dentistService.UpdateDentistCheckByMemberIDAsync(editDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}