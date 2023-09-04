using System.Threading.Tasks;
using HelpCheck_API.Dtos.Appointments;
using HelpCheck_API.Services.Appointments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpCheck_API.Controllers.MobileApps
{
    [Authorize]
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointment([FromQuery] FilterAppointmentDto filterDto)
        {
            var result = await _appointmentService.GetAllAppointmentSettingAsync(filterDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("AppointmentMobile")]
        public async Task<IActionResult> AppointmentMobile([FromQuery] FilterAppointmentDto filterDto)
        {
            var result = await _appointmentService.GetAllAppointmentSettingMobileAsync(filterDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("MyAppointment")]
        public async Task<IActionResult> GetAppointmentByID()
        {
            string accessToken = GetAccessTokenFromHeader();
            var result = await _appointmentService.GetAppointmentSettingByAccessTokenAsync(accessToken);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost("Reserve")]
        public async Task<IActionResult> CreateReserveAppointment([FromBody] AddBookingAppointmentDto addDto)
        {
            addDto.AccessToken = GetAccessTokenFromHeader();
            addDto.IsBooked = false;
            var result = await _appointmentService.BookingAppointmentAsync(addDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost("Booking")]
        public async Task<IActionResult> CreateAppointment([FromBody] AddBookingAppointmentDto addDto)
        {
            addDto.AccessToken = GetAccessTokenFromHeader();
            addDto.IsBooked = true;
            var result = await _appointmentService.BookingAppointmentAsync(addDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPut("UnReserve/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UnReserveDto unReserveDto)
        {
            string accessToken = GetAccessTokenFromHeader();
            var result = await _appointmentService.UnReserveAppointmentAsync(id, accessToken);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("Count/{id}")]
        public async Task<IActionResult> GetCountAppointmentByID(int id)
        {
            var result = await _appointmentService.GetCountAppointmentSettingByIDAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpDelete("CancelBooked/{appointmentId}")]
        public async Task<IActionResult> CancelBookedByAppointmentID(int appointmentId)
        {
            string accessToken = GetAccessTokenFromHeader();
            var result = await _appointmentService.CancelBookedByAppointmentIDAsync(appointmentId, accessToken);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}