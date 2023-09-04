using HelpCheck_API.Dtos.Users;
using HelpCheck_API.Helpers;
using HelpCheck_API.Repositories.Users;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HelpCheck_API.Services
{
    public class BaseService
    {
        private static readonly AppSettingHelper appSettingHelper = new AppSettingHelper();
        private readonly IUserRepository _userRepository;
        public BaseService()
        {
        }

        public BaseService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected async Task<LoginResponseDto> CreateTokenUser(string userToken, string userId, string userName, int? roleId)
        {
            string jwtKey = appSettingHelper.GetConfiguration("JwtKey");

            List<Claim> claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userToken),
                    new Claim(JwtRegisteredClaimNames.Jti, userId)
                };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime issues = DateTime.Now;
            DateTime expires = issues.AddMinutes(Convert.ToInt32(appSettingHelper.GetConfiguration("JwtExpireMin")));

            JwtSecurityToken token = new JwtSecurityToken(
                 issuer: appSettingHelper.GetConfiguration("JwtIssuer"),
                 audience: appSettingHelper.GetConfiguration("JwtAudience"),
                 claims: claims,
                 expires: expires,
                 signingCredentials: credentials
             );

            LoginResponseDto loginResponse = new LoginResponseDto()
            {
                UserID = userId,
                UserName = userName,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresIn = issues,
                ExpireDate = expires,
                RoleID = roleId
            };
            return await Task.FromResult<LoginResponseDto>(loginResponse);
        }

        protected async Task<string> GetUserFullNameByAccessTokenAsync(string accessToken)
        {
            try
            {
                var user = await _userRepository.GetUserByAccessTokenAsync(accessToken);

                return user.TitleName + " " + user.FirstName + " " + user.LastName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
