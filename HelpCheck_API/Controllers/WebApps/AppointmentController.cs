using System.Threading.Tasks;
using HelpCheck_API.Dtos.Appointments;
using HelpCheck_API.Services.Appointments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpCheck_API.Controllers.WebApps
{
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointmentSetting([FromQuery] FilterAppointmentDto filterDto)
        {
            var result = await _appointmentService.GetAllAppointmentSettingAsync(filterDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentSettingByID(int id)
        {
            var result = await _appointmentService.GetAppointmentSettingByIDAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointmentSetting([FromBody] AddAppointmentSettingDto addDto)
        {
            addDto.AccessToken = GetAccessTokenFromHeader();
            var result = await _appointmentService.CreateAppointmentSettingAsync(addDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAppointmentSetting(int id, [FromBody] EditAppointmentSettingDto editDto)
        {
            editDto.AccessToken = GetAccessTokenFromHeader();
            editDto.ID = id;
            var result = await _appointmentService.UpdateAppointmentSettingAsync(editDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("Count/{id}")]
        public async Task<IActionResult> GetCountAppointmentSettingByID(int id)
        {
            var result = await _appointmentService.GetCountAppointmentSettingByIDAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentSettingByID(int id)
        {
            var result = await _appointmentService.DeleteAppointmentSettingAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}