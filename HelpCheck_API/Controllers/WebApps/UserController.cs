using HelpCheck_API.Dtos.Users;
using HelpCheck_API.Services.Patients;
using HelpCheck_API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;
        public UserController(IUserService userService, IPatientService patientService)
        {
            _userService = userService;
            _patientService = patientService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] FilterUserDto filterUserDto)
        {
            var result = await _userService.GetUsersAsync(filterUserDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var accessToken = GetAccessTokenFromHeader();
            var result = await _userService.GetUserProfileByAccessTokenAsync(accessToken);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] AddUserDto addUserDto)
        {
            var result = await _userService.RegisterAsync(addUserDto, false);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUserByID(string userId, [FromBody] EditUserDto editUserDto)
        {
            editUserDto.UserID = userId;
            var result = await _userService.UpdateUserByIDAsync(editUserDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserByID(string userId)
        {
            var result = await _userService.DeleteUserByIDAsync(userId);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpPatch("IsNotActiveUser/{userId}")]
        public async Task<IActionResult> UpdateUserNotActiveAsync(string userId)
        {
            var result = await _userService.UpdateUserNotActiveAsync(userId);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpPatch("Info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] EditUserDto editUserDto)
        {
            editUserDto.AccessToken = GetAccessTokenFromHeader();
            var result = await _userService.UpdateUserInfoAsync(editUserDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("Profile/{id}")]
        public async Task<IActionResult> GetUserProfileByID(int id)
        {
            var result = await _userService.GetUserProfileByIDAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPost("UpdatePreName")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePreName()
        {
            var allUsers = await _userService.GetAllUsers();

            foreach (var user in allUsers)
            {
                if (string.IsNullOrWhiteSpace(user.IDCard))
                    continue;

                var userOther = await _userService.GetUserByOrtherServiceAsync(user.IDCard);
                if (userOther.IsSuccess)
                {
                    user.TitleName = ((GetUserDto)userOther.Data).TitleName;

                    var resultUpdated = await _userService.PatchUserAsync(user);
                    if (!resultUpdated.IsSuccess)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, resultUpdated.Data);
                    }
                }
                else
                {
                    continue;
                }
            }

            return Ok("Success");
        }
    }
}