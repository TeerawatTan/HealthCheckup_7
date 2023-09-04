using HelpCheck_API.Dtos.Patients;
using HelpCheck_API.Services.Patients;
using HelpCheck_API.Services.Physicals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    public class PhysicalController : BaseController
    {
        private readonly IPhysicalService _physicalService;
        private readonly IPatientService _patientService;
        public PhysicalController(IPhysicalService physicalService, IPatientService patientService)
        {
            _physicalService = physicalService;
            _patientService = patientService;
        }

        [HttpGet("DropdownList")]
        public async Task<IActionResult> GetDropdownList()
        {
            var result = await _physicalService.GetPhysicalDropdownListAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhysicalExaminationMaster(int id)
        {
            var result = await _physicalService.GetPhysicalExaminationMasterListByPhysicalSetIDAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePhysicalPatient([FromBody] AddPhysicalDto addPhysicalDto)
        {
            addPhysicalDto.AccessToken = GetAccessTokenFromHeader();
            var result = await _patientService.CreatePhysicalPatientAsync(addPhysicalDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{memberId}")]
        public async Task<IActionResult> CreatePhysicalPatient(int memberId, [FromBody] EditPhysicalDto editPhysicalDto)
        {
            editPhysicalDto.AccessToken = GetAccessTokenFromHeader();
            var result = await _patientService.UpdatePhysicalPatientAsync(editPhysicalDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}