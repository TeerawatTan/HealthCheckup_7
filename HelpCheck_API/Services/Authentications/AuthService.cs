using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.OtpAndSendMails;
using HelpCheck_API.Dtos.Users;
using HelpCheck_API.Helpers;
using HelpCheck_API.Repositories.Users;
using HelpCheck_API.Services.Users;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Authentications
{
    public class AuthService : BaseService, IAuthService
    {
        private static readonly AppSettingHelper appSettingHelper = new AppSettingHelper();

        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        public AuthService(IUserRepository userRepository, IUserService userService) : base()
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        public async Task<ResultResponse> SignInAsync(LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto is not null && !string.IsNullOrWhiteSpace(loginRequestDto.Username) && !string.IsNullOrWhiteSpace(loginRequestDto.Password))
            {
                try
                {
                    if (!loginRequestDto.Username.Contains("/"))
                    {
                        var cli = new RestClient("http://dev34.pmk.ac.th:8080/ords/dev/api/Duser/" + loginRequestDto.Username)
                        {
                            Timeout = -1
                        };
                        var req = new RestRequest(Method.GET);
                        req.AddHeader("Content-Type", "application/json");
                        IRestResponse response = await cli.ExecuteAsync(req);

                        // Log
                        Console.WriteLine("### Response: {0}", response);
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine("### Before map content: {0}", response.Content);

                        DUserDto result = JsonConvert.DeserializeObject<DUserDto>(response.Content);

                        // Log
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine("### After map content to model: " + result.Data);
                        Console.WriteLine("-----------------------------------");
                        if (result.Data is not null && result.Data.Count > 0)
                        {
                            DUserDetailDto data = result.Data.FirstOrDefault();
                            if (string.IsNullOrWhiteSpace(data.Role))
                            {
                                return new ResultResponse()
                                {
                                    IsSuccess = false,
                                    Data = "Role is not null"
                                };
                            }

                            if (loginRequestDto.Password != data.Checkword)
                            {
                                return new ResultResponse()
                                {
                                    IsSuccess = false,
                                    Data = Constant.STATUS_PASSWORD_NOT_MATCH
                                };
                            }
                            else
                            {
                                if (!await _userRepository.CheckUserExists(loginRequestDto.Username))
                                {
                                    AddUserDto addUser = new AddUserDto()
                                    {
                                        Username = data.User,
                                        Email = data.User,
                                        FirstName = data.Name,
                                        Password = data.Checkword,
                                        RoleID = Convert.ToInt32(data.Role)
                                    };

                                    var resultAddUser = await _userService.RegisterAsync(addUser, false);
                                    if (!resultAddUser.IsSuccess)
                                    {
                                        return resultAddUser;
                                    }
                                }
                            }
                        }
                    }

                    var user = await _userRepository.GetUserInfoByUserNameAsync(loginRequestDto.Username);

                    if (user is null)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = Constant.STATUS_DATA_NOT_FOUND
                        };
                    }

                    //if (user.RoleID != 5)
                    //{
                    //    return new ResultResponse()
                    //    {
                    //        IsSuccess = false,
                    //        Data = Constant.DATA_STATUS_FORBIDDEN
                    //    };
                    //}

                    if (!user.IsActive || !PasswordHasher.Check(user.Password, loginRequestDto.Password).Verified)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = Constant.STATUS_PASSWORD_NOT_MATCH
                        };
                    }

                    string accessToken = CryptoEngine.Encrypt(Guid.NewGuid().ToString());
                    user.Token = accessToken;
                    user.ExpireDate = DateTime.Now.AddMinutes(Convert.ToInt32(appSettingHelper.GetConfiguration("JwtExpireMin")));
                    var resultUpdate = await _userRepository.UpdateUserAsync(user);
                    if (resultUpdate == null || resultUpdate != Constant.STATUS_SUCCESS)
                    {
                        throw new Exception(resultUpdate);
                    }

                    var res = await CreateTokenUser(user.Token, user.UserID, user.UserName, user.RoleID);

                    return new ResultResponse()
                    {
                        IsSuccess = true,
                        Data = res
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return new ResultResponse()
            {
                IsSuccess = false,
                Data = Constant.STATUS_DATA_NOT_FOUND
            };
        }

        public async Task<ResultResponse> ResetPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userRepository.GetUserInfoByUserNameAsync(forgotPasswordDto.Username);

            if (user is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            try
            {
                string ranNum = RandomUtility.RandomNumber(0, 999999).ToString().PadLeft(6, '0');
                string hashPassword = PasswordHasher.Hash(user.IDCard.Substring(user.IDCard.Length - 4));
                string hashCode = PasswordHasher.Hash(ranNum);

                user.Password = hashPassword;
                user.IsActive = true;
                user.ExpireDate = DateTime.Now.AddMinutes(30);
                user.Token = hashCode;

                var status = await _userRepository.UpdateUserAsync(user);

                if (status != Constant.STATUS_SUCCESS)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = status
                    };
                }

                //if (!string.IsNullOrWhiteSpace(user.Email))
                //{
                //    var config = new ConfigurationBuilder()
                //                                .SetBasePath(Directory.GetCurrentDirectory())
                //                                .AddJsonFile("appsettings.json").Build();

                //    MailRequest mailRequest = new MailRequest()
                //    {
                //        Subject = "รหัสรักษาความปลอดภัย",
                //        Sender = config["SenderSubject"],
                //        SenderEmail = config["EmailSender"],
                //        ReceiverEmail = user.Email,
                //        Details = "<div style='font-size: 22px; color: #0071FC;'>รหัสรักษาความปลอดภัย</div><br /><div>โปรดใช้รหัสต่อไปนี้กับบัญชี " + forgotPasswordDto.Username + "</div>" +
                //        "<br /><div>รหัสความปลอดภัย : <span style='color:red;'>" + ranNum + "</span></div><br /><div>รหัสยืนยันจะหมดอายุภายใน 30 นาที</div>"
                //    };

                //    ResultResponse mailResponse = SendEmailService.SendMail(mailRequest);

                //    if (!mailResponse.IsSuccess)
                //    {
                //        return new ResultResponse()
                //        {
                //            IsSuccess = false,
                //            Data = mailResponse.Data
                //        };

                //    }
                //}

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = status
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }

        public async Task<ResultResponse> SetPasswordAsync(SetPasswordDto setPasswordDto)
        {
            var user = await _userRepository.GetUserInfoByUserNameAsync(setPasswordDto.Email);
            if (user is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            // if (!PasswordHasher.Check(user.Token, setPasswordDto.Code).Verified || user.ExpireDate < DateTime.Now)
            // {
            //     return new ResultResponse()
            //     {
            //         IsSuccess = false,
            //         Data = Constant.DATA_CODE_EXPIRE
            //     };
            // }

            string hashPassword = PasswordHasher.Hash(setPasswordDto.NewPassword);

            user.Password = hashPassword;
            user.IsActive = true;

            var status = await _userRepository.UpdateUserAsync(user);

            if (status != Constant.STATUS_SUCCESS)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = status
                };
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = Constant.STATUS_SUCCESS
            };
        }

        public async Task<ResultResponse> RefreshTokenAsync(string userName)
        {
            var user = await _userRepository.GetUserInfoByUserNameAsync(userName);
            if (user is null || string.IsNullOrWhiteSpace(user.Token))
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            string accessToken = CryptoEngine.Encrypt(Guid.NewGuid().ToString());
            user.Token = accessToken;
            user.ExpireDate = DateTime.Now.AddMinutes(Convert.ToInt32(appSettingHelper.GetConfiguration("JwtExpireMin")));
            await _userRepository.UpdateUserAsync(user);

            var result = await CreateTokenUser(user.Token, user.UserID, userName, user.RoleID);

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }
    }
}
