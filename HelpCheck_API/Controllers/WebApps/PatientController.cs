using HelpCheck_API.Dtos.Patients;
using HelpCheck_API.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    [Authorize]
    public class PatientController : BaseController
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatient([FromQuery] FilterPatientAppointmentDto filterPatientAppointmentDto)
        {
            var result = await _patientService.GetPatientAppointmentAsync(filterPatientAppointmentDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("Appointment")]
        public async Task<IActionResult> GetAllPatientByAppointment([FromQuery] FilterPatientDto filterDto)
        {
            var result = await _patientService.GetAllPatientAppointmentAsync(filterDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("Physical/{memberId}")]
        public async Task<IActionResult> GetPhysicalByMemberID(int memberId)
        {
            var result = await _patientService.GetPhysicalByMemberIDAsync(memberId);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("GetAppointmentDate/{memberId}")]
        public async Task<IActionResult> GetAppointmentDateByMemberID(int memberId)
        {
            var result = await _patientService.GetAppointmentDateByMemberIDAsync(memberId);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpGet("GetXRayResult/{idcard}")]
        public async Task<IActionResult> GetXRayResultByIdCard(string idcard)
        {
            var result = await _patientService.GetXRayResultByIdCard(idcard);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpGet("GetBloodResult/{id}/{year}")]
        public async Task<IActionResult> GetBloodResultByHn(string id, string year)
        {
            string hn = string.Format("{0}/{1}", id, year);
            var result = await _patientService.GetBloodResultByHn(hn);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpGet("GetLabSmear/{id}/{year}")]
        public async Task<IActionResult> GetLabSmearByHn(string id, string year)
        {
            string hn = string.Format("{0}/{1}", id, year);
            var result = await _patientService.GetLabSmearByHn(hn);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}
