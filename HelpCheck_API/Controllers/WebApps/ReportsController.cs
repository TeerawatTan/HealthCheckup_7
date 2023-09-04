using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using HelpCheck_API.Dtos.Reports;
using HelpCheck_API.Services.Appointments;
using HelpCheck_API.Services.Reports;
using System.Collections.Generic;
using HelpCheck_API.Dtos.Patients;
using System.Threading;
using HelpCheck_API.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace HelpCheck_API.Controllers.WebApps
{
    [Authorize]
    public class ReportsController : BaseController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IReportService _reportService;

        public ReportsController(IAppointmentService appointmentService, IReportService reportService)
        {
            _appointmentService = appointmentService;
            _reportService = reportService;
        }

        [HttpGet("Appointment")]
        public async Task<ActionResult<List<GetAppointmentReportDto>>> GetAppointmentByDateReport(string date)
        {
            try
            {
                string token = GetAccessTokenFromHeader();

                DateTime? dt = null;

                if (string.IsNullOrWhiteSpace(date))
                {
                    DateTime now = DateTime.Now;
                    dt = new DateTime(now.Year, now.Month, now.Day);
                }
                else
                {
                    dt = DateTime.Parse(date);
                }

                var result = await _appointmentService.GetAppointmentByDateAsync(dt.Value);

                if (!result.IsSuccess)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("AllPatient")]
        public async Task<ActionResult<PageginationResultResponse<List<GetReportDto>>>> GetReportAllPatient([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            string accessToken = GetAccessTokenFromHeader();
            var result = await _reportService.GetReportAsync(pageNumber, pageSize, cancellationToken);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }

            return Ok(result.Data);
        }

        [HttpGet("Daily/HealthCheck")]
        public async Task<ActionResult<PageginationResultResponse<GetAllPatientAppointmentDto>>> GetReportDailyHealthCheck(string date, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            string accessToken = GetAccessTokenFromHeader();
            // wait rule check

            DateTime? dt = null;

            if (string.IsNullOrWhiteSpace(date))
            {
                DateTime now = DateTime.Now;
                dt = new DateTime(now.Year, now.Month, now.Day);
            }
            else
            {
                dt = DateTime.Parse(date);
            }

            var filter = new FilterPatientDto()
            {
                AppointmentDate = dt
            };

            var result = await _reportService.GetReportDailyHealthCheck(filter, pageNumber, pageSize);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }

            return Ok(result.Data);
        }

        [HttpGet("Daily/PsychiatristCheck")]
        public async Task<ActionResult<PageginationResultResponse<List<GetReportDailyPsychiatristCheckDto>>>> GetRepoDailyPsychiatristCheck([FromQuery] string date, [FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            DateTime? dt = null;

            if (string.IsNullOrWhiteSpace(date))
            {
                DateTime now = DateTime.Now;
                dt = new DateTime(now.Year, now.Month, now.Day);
            }
            else
            {
                dt = DateTime.Parse(date);
            }

            var result = await _reportService.GetReportDailyPsychiatristCheck(dt.Value, pageNumber, pageSize, cancellationToken);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }

            return Ok(result.Data);
        }

        [HttpGet("DentistCheck")]
        public async Task<ActionResult<List<GetReportDentistCheckDto>>> GetReportDentistCheck()
        {
             var result = await _reportService.GetReportDentistCheck();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }

            return Ok(result.Data);
        }
    }
}
