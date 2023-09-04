using HelpCheck_API.Dtos.Users;
using HelpCheck_API.Services.Authentications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequestDto loginRequestDto)
        {
            var result = await _authService.SignInAsync(loginRequestDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var result = await _authService.ResetPasswordAsync(forgotPasswordDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost("SetPassword")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto setPasswordDto)
        {
            var result = await _authService.SetPasswordAsync(setPasswordDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string userName)
        {
            string token = GetAccessTokenFromHeader();
            var result = await _authService.RefreshTokenAsync(userName);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}
