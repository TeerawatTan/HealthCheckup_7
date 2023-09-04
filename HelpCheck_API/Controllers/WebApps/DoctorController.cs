using HelpCheck_API.Dtos.Doctors;
using HelpCheck_API.Services.Doctors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    [Authorize]
    public class DoctorController : BaseController
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("{memberId}")]
        public async Task<IActionResult> GetDoctorCheckByMemberID(int memberId)
        {
            var result = await _doctorService.GetDoctorCheckByMemberIDAsync(memberId);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctorCheck([FromBody] AddDoctorCheckDto addDto)
        {
            addDto.AccessToken = GetAccessTokenFromHeader();

            if (addDto.MemberID <= 0)
            {
                return BadRequest("Member is not null");
            }

            var result = await _doctorService.CreateDoctorCheckAsync(addDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{memberId}")]
        public async Task<IActionResult> UpdateDoctorCheck(int memberId, [FromBody] EditDoctorCheckDto editDto)
        {
            editDto.MemberID = memberId;
            editDto.AccessToken = GetAccessTokenFromHeader();
            var result = await _doctorService.UpdateDoctorCheckByMemberIDAsync(editDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}