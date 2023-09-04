using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Authentications
{
    public interface IAuthService
    {
        Task<ResultResponse> SignInAsync(LoginRequestDto loginRequestDto);
        Task<ResultResponse> ResetPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<ResultResponse> SetPasswordAsync(SetPasswordDto setPasswordDto);
        Task<ResultResponse> RefreshTokenAsync(string userName);
    }
}
