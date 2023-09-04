using HelpCheck_API.Dtos.Users;
using HelpCheck_API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.MobileApps
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUsersAsync(new FilterUserDto());
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
            //if (IsValidEmail(addUserDto.Username))
            //{
            //    addUserDto.Email = addUserDto.Username;
            //}
            //else if (addUserDto.Username.Length >= 9 && addUserDto.Username.Length < 13)
            //{
            //    addUserDto.PhoneNo = addUserDto.Username;
            //}
            //else if (addUserDto.Username.Length == 13)
            //{
            //    addUserDto.IDCard = addUserDto.Username;
            //}
            //else
            //{
            //    addUserDto.Hn = addUserDto.Username;
            //}

            var result = await _userService.RegisterAsync(addUserDto, true);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        //[Authorize]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserByID(string userId)
        //{
        //    var result = await _userService.DeleteUserByIDAsync(userId);
        //    if (!result.IsSuccess)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
        //    }
        //    return Ok(result.Data);
        //}

        [Authorize]
        [HttpPut("IsNotActiveUser/{id}")]
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
        [HttpPut("Info")]
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

        [HttpGet("{idCard}")]
        public async Task<IActionResult> PullUserInfo(string idCard)
        {
            var result = await _userService.GetUserByOrtherServiceAsync(idCard);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}
