using HelpCheck_API.Dtos.Patients;
using HelpCheck_API.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.MobileApps
{
    [Authorize]
    public class PatientController : BaseController
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("Physical")]
        public async Task<IActionResult> GetPhysicalByMemberID()
        {
            string accessToken = GetAccessTokenFromHeader();
            var result = await _patientService.GetPhysicalByTokenAsync(accessToken);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        // [HttpGet("GetAppointmentDate/{memberId}")]
        // public async Task<IActionResult> GetAppointmentDateByMemberID(int memberId)
        // {
        //     var result = await _patientService.GetAppointmentDateByMemberIDAsync(memberId);
        //     if (!result.IsSuccess)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
        //     }
        //     return Ok(result.Data);
        // }
    }
}
