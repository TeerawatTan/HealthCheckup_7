using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Patients;
using HelpCheck_API.Dtos.Users;
using HelpCheck_API.Helpers;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.MasterJobTypes;
using HelpCheck_API.Repositories.MasterTreatments;
using HelpCheck_API.Repositories.MasterWorkPlaces;
using HelpCheck_API.Repositories.QuestionAndChoices;
using HelpCheck_API.Repositories.Users;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Users
{
    public class UserService : IUserService
    {
        #region Injection
        private static readonly AppSettingHelper _appSetting = new AppSettingHelper();
        private readonly IUserRepository _userRepository;
        private readonly IMasterJobTypeRepository _masterJobType;
        private readonly IMasterWorkPlaceRepository _masterWorkPlace;
        private readonly IQuestionAndChoiceRepository _questionAndChoiceRepository;
        private readonly IMasterTreatmentRepository _masterTreatmentRepository;
        #endregion

        public UserService(
            IUserRepository userRepository,
            IMasterJobTypeRepository masterJobType,
            IMasterWorkPlaceRepository masterWorkPlace,
            IQuestionAndChoiceRepository questionAndChoiceRepository,
            IMasterTreatmentRepository masterTreatmentRepository)
        {
            _userRepository = userRepository;
            _masterJobType = masterJobType;
            _masterWorkPlace = masterWorkPlace;
            _questionAndChoiceRepository = questionAndChoiceRepository;
            _masterTreatmentRepository = masterTreatmentRepository;
        }

        public async Task<ResultResponse> RegisterAsync(AddUserDto addUserDto, bool? isMobile)
        {
            if (string.IsNullOrWhiteSpace(addUserDto.Username))
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }
            if (await _userRepository.CheckUserExists(addUserDto.Username))
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_USER_ALREADY_EXISTS
                };
            }

            try
            {
                string hashPassword = PasswordHasher.Hash(addUserDto.Password);

                var user = new User();
                user.UserID = Guid.NewGuid().ToString();
                user.TitleName = addUserDto.TitleName;
                user.FirstName = addUserDto.FirstName;
                user.LastName = addUserDto.LastName;
                user.Email = addUserDto.Email;
                user.Password = hashPassword;
                user.IDCard = addUserDto.IDCard;
                user.BirthDate = addUserDto.BirthDate;
                user.Gender = addUserDto.Gender;
                user.Agency = addUserDto.Agency;
                user.WorkPlaceID = addUserDto.WorkPlaceID;
                user.JobTypeID = addUserDto.JobTypeID;
                user.PhoneNo = addUserDto.PhoneNo;
                user.IsActive = true;
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = addUserDto.Email;
                user.Hn = addUserDto.Hn;
                user.RoleID = addUserDto.RoleID != null ? addUserDto.RoleID : isMobile.HasValue ? isMobile.Value ? 5 : 0 : null;
                user.TreatmentID = addUserDto.TreatmentID;
                user.UserName = addUserDto.Username;

                // Upload Image
                string path = "";
                if (addUserDto.ImageFile != null && addUserDto.ImageFile.Length > 0)
                {
                    string newFileName = DateTime.Now.ToString("yyMMddhhmmssfff");
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, newFileName + "_" + addUserDto.ImageFile.FileName), FileMode.Create))
                    {
                        await addUserDto.ImageFile.CopyToAsync(fileStream);
                    }
                }

                user.ImagePath = _appSetting.GetConfiguration("ApiUrl") + path;

                string status = await _userRepository.CreateUserAsync(user);
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

        public async Task<ResultResponse> DeleteUserByIDAsync(string userId)
        {
            var user = await _userRepository.GetUserByUserIDAsync(userId);

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
                var status = await _userRepository.DeleteUserAsync(user);
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

        public async Task<ResultResponse> UpdateUserNotActiveAsync(string userId)
        {
            var user = await _userRepository.GetUserByUserIDAsync(userId);

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
                user.IsActive = false;
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

        public async Task<ResultResponse> GetUserProfileByAccessTokenAsync(string accessToken)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(accessToken);
            if (user is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            var userDto = new GetUserDto
            {
                ID = user.ID,
                UserID = user.UserID,
                TitleName = user.TitleName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.TitleName + " " + user.FirstName + " " + user.LastName,
                BirthDate = user.BirthDate,
                Age = DateTimeUtility.CalculateAge(user.BirthDate),
                Gender = user.Gender,
                IDCard = user.IDCard,
                Hn = user.Hn,
                PhoneNo = user.PhoneNo,
                Agency = user.Agency,
                JobTypeID = user.JobTypeID,
                JobTypeName = _masterJobType.GetMasterJobTypeNameByID(user.JobTypeID ?? 0),
                WorkPlaceID = user.WorkPlaceID,
                WorkPlaceName = _masterWorkPlace.GetMasterWorkPlaceNameByID(user.WorkPlaceID ?? 0),
                IsActive = user.IsActive,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.CreatedDate,
                TreatmentID = user.TreatmentID,
                TreatmentName = _masterTreatmentRepository.GetMasterTreatmentNameByID(user.TreatmentID ?? 0),
                UserName = user.UserName,
                ImageUrl = user.ImagePath
            };

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = userDto
            };
        }

        public async Task<ResultResponse> GetUsersAsync(FilterUserDto filterUserDto)
        {
            var users = await _userRepository.GetUsersAsync(filterUserDto);

            var user = users.Select(s => new GetUserDto()
            {
                ID = s.ID,
                UserID = s.UserID,
                TitleName = s.TitleName,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FullName = s.TitleName + " " + s.FirstName + " " + s.LastName,
                BirthDate = s.BirthDate,
                Age = s.BirthDate.HasValue ? DateTimeUtility.CalculateAge(s.BirthDate) : 0,
                Gender = s.Gender,
                IDCard = s.IDCard,
                Hn = s.Hn,
                PhoneNo = s.PhoneNo,
                Agency = s.Agency,
                JobTypeID = s.JobTypeID,
                JobTypeName = s.JobTypeID != null ? _masterJobType.GetMasterJobTypeNameByID(s.JobTypeID ?? 0) : string.Empty,
                WorkPlaceID = s.WorkPlaceID,
                WorkPlaceName = s.WorkPlaceID != null ? _masterWorkPlace.GetMasterWorkPlaceNameByID(s.WorkPlaceID ?? 0) : string.Empty,
                IsActive = s.IsActive,
                CreatedBy = s.CreatedBy,
                CreatedDate = s.CreatedDate,
                TreatmentID = s.TreatmentID,
                TreatmentName = s.TreatmentID != null ? _masterTreatmentRepository.GetMasterTreatmentNameByID(s.TreatmentID ?? 0) : string.Empty,
                UserName = s.UserName,
                ImageUrl = s.ImagePath
            });

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = user
            };
        }

        public async Task<ResultResponse> UpdateUserInfoAsync(EditUserDto editUserDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(editUserDto.AccessToken);

            if (user is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }
            try
            {
                user.TitleName = editUserDto.TitleName;
                user.FirstName = editUserDto.FirstName;
                user.LastName = editUserDto.LastName;
                user.BirthDate = editUserDto.BirthDate;
                user.Gender = editUserDto.Gender;
                user.Agency = editUserDto.Agency;
                user.WorkPlaceID = editUserDto.WorkPlaceID;
                user.JobTypeID = editUserDto.JobTypeID;
                user.PhoneNo = editUserDto.PhoneNo;
                user.Hn = editUserDto.Hn;
                user.UpdatedBy = user.Email;
                user.TreatmentID = editUserDto.TreatmentID;
                user.UserName = user.UserName;
                user.ImagePath = editUserDto.ImageUrl;
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

        public async Task<ResultResponse> UpdateUserByIDAsync(EditUserDto editUserDto)
        {
            var userByToken = await _userRepository.GetUserByAccessTokenAsync(editUserDto.AccessToken);

            var user = await _userRepository.GetUserByUserIDAsync(editUserDto.UserID);

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
                user.TitleName = editUserDto.TitleName;
                user.FirstName = editUserDto.FirstName;
                user.LastName = editUserDto.LastName;
                //user.IDCard = editUserDto.IDCard;
                user.BirthDate = editUserDto.BirthDate;
                user.Gender = editUserDto.Gender;
                user.Agency = editUserDto.Agency;
                user.WorkPlaceID = editUserDto.WorkPlaceID;
                user.JobTypeID = editUserDto.JobTypeID;
                user.PhoneNo = editUserDto.PhoneNo;
                user.Hn = editUserDto.Hn;
                user.UpdatedBy = userByToken.Email;
                user.TreatmentID = userByToken.TreatmentID;
                user.UserName = userByToken.UserName;
                user.ImagePath = editUserDto.ImageUrl;

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

        public async Task<ResultResponse> GetUserProfileByIDAsync(int id)
        {
            User user = await _userRepository.GetUserByIDAsync(id);
            if (user is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            int count = await _questionAndChoiceRepository.CountQuestionAndChoiceAsync(id);
            string ans40 = "-", ans2q = "NEGATIVE";
            int ans9q = 0, ansSt5 = 0, ans8q = 0, ansGHQ28Group1 = 0, ansGHQ28Group2 = 0, ansGHQ28Group3 = 0, ansGHQ28Group4 = 0;
            var q40 = await _questionAndChoiceRepository.GetAnswerByQuestIDAsync(id, 40);
            if (q40 is not null)
                ans40 = q40.AnswerKeyIn;
            var q41 = await _questionAndChoiceRepository.GetAnswerByQuestIDAsync(id, 41);
            var q42 = await _questionAndChoiceRepository.GetAnswerByQuestIDAsync(id, 42);
            if ((q41 is not null && q41.ChoiceID == 35) || (q42 is not null && q42.ChoiceID == 35))
            {
                ans2q = "POSITIVE";
            }
            var question = await _questionAndChoiceRepository.GetAnswerAsync(id, DateTime.Now.Year);
            if (question is not null && question.Count > 0)
            {
                var question9q = question.Where(w => (new[] { 47, 48, 49, 50 }).Contains(w.ChoiceID)).ToList();
                if (question9q is not null && question9q.Count > 0)
                {
                    ans9q += question9q.Sum(s => s.Score ?? 0);
                }
                var questionSt5 = question.Where(w => (new[] { 51, 52, 53, 54 }).Contains(w.ChoiceID)).ToList();
                if (questionSt5 is not null && questionSt5.Count > 0)
                {
                    ansSt5 += questionSt5.Sum(s => s.Score ?? 0);
                }
                //-------- Add new ----------------
                // 8Q
                var question8q = question.Where(w => (new [] { "55","56","57","58","59","60","61","62","63" }).Contains(w.QuestionNum)).ToList();
                if (question8q is not null && question8q.Count > 0)
                {
                    ans8q += question8q.Sum(s => s.Score ?? 0);
                }
                // GHQ28 1-7
                var questionGHQ28Group1 = question.Where(w => (new[] { "64","65","66","67","68","69","70" }).Contains(w.QuestionNum)).ToList();
                if (questionGHQ28Group1 is not null && questionGHQ28Group1.Count > 0)
                {
                    ansGHQ28Group1 += questionGHQ28Group1.Sum(s => s.Score ?? 0);
                }
                // GHQ28 8-14
                var questionGHQ28Group2 = question.Where(w => (new[] { "71","72","73","74","75","76","77" }).Contains(w.QuestionNum)).ToList();
                if (questionGHQ28Group2 is not null && questionGHQ28Group2.Count > 0)
                {
                    ansGHQ28Group2 += questionGHQ28Group2.Sum(s => s.Score ?? 0);
                }
                // GHQ28 15-21
                var questionGHQ28Group3 = question.Where(w => (new[] { "78","79","80","81","82","83","84" }).Contains(w.QuestionNum)).ToList();
                if (questionGHQ28Group3 is not null && questionGHQ28Group3.Count > 0)
                {
                    ansGHQ28Group3 += questionGHQ28Group3.Sum(s => s.Score ?? 0);
                }
                // GHQ28 22-28
                var questionGHQ28Group4 = question.Where(w => (new[] { "85","86","87","88","89","90","91" }).Contains(w.QuestionNum)).ToList();
                if (questionGHQ28Group4 is not null && questionGHQ28Group4.Count > 0)
                {
                    ansGHQ28Group4 += questionGHQ28Group4.Sum(s => s.Score ?? 0);
                }
            }

            var userDto = new GetUserDto
            {
                UserID = user.UserID,
                TitleName = user.TitleName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.TitleName + " " + user.FirstName + " " + user.LastName,
                BirthDate = user.BirthDate,
                Age = DateTimeUtility.CalculateAge(user.BirthDate),
                Gender = user.Gender,
                IDCard = user.IDCard,
                Hn = user.Hn,
                PhoneNo = user.PhoneNo,
                Agency = user.Agency,
                JobTypeID = user.JobTypeID,
                JobTypeName = _masterJobType.GetMasterJobTypeNameByID(user.JobTypeID ?? 0),
                WorkPlaceID = user.WorkPlaceID,
                WorkPlaceName = _masterWorkPlace.GetMasterWorkPlaceNameByID(user.WorkPlaceID ?? 0),
                IsActive = user.IsActive,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.CreatedDate,
                Count = count,
                TreatmentID = user.TreatmentID,
                TreatmentName = _masterTreatmentRepository.GetMasterTreatmentNameByID(user.TreatmentID ?? 0),
                UserName = user.UserName,
                Question40 = ans40,
                Question2q = ans2q,
                Question9q = ans9q,
                ImageUrl = user.ImagePath,
                QuestionSt5 = ansSt5,
                Question8q = ans8q,
                QuestionGHQ28Group1 = ansGHQ28Group1,
                QuestionGHQ28Group2 = ansGHQ28Group2,
                QuestionGHQ28Group3 = ansGHQ28Group3,
                QuestionGHQ28Group4 = ansGHQ28Group4
            };

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = userDto
            };
        }

        public async Task<ResultResponse> GetUserByOrtherServiceAsync(string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_INVALID_REQUEST_DATA
                };
            }

            try
            {
                //if (await _userRepository.CheckUserExists(idCard))
                //{
                //    return new ResultResponse()
                //    {
                //        IsSuccess = false,
                //        Data = Constant.STATUS_USER_ALREADY_EXISTS
                //    };
                //}

                //var cli = new RestClient("http://202.28.80.34:8080/ords/dev/patient/check/" + idCard.Trim().Replace("-", ""))
                //{
                //    Timeout = -1
                //};
                //var req = new RestRequest(Method.GET);
                //req.AddHeader("Content-Type", "application/json");
                //IRestResponse response = cli.Execute(req);
                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                //    var data = JsonConvert.DeserializeObject<GetPatientFromApiDto>(response.Content);
                //    var userDto = new GetUserDto();
                //    if (data != null)
                //    {
                //        DateTime? dataBirthDate = DateTime.Parse(data.birth_year + "-" + data.birth_month + "-" + data.birth_day, System.Globalization.CultureInfo.InvariantCulture);
                //        userDto.IDCard = !string.IsNullOrWhiteSpace(data.id_card) ? idCard.Trim().Replace("-", "") : idCard;
                //        userDto.Hn = data.hn;
                //        userDto.TitleName = !string.IsNullOrWhiteSpace(data.prename) ? data.prename.Trim() : null;
                //        userDto.FirstName = data.name;
                //        userDto.LastName = data.surname;
                //        userDto.FullName = data.prename + " " + data.name + " " + data.surname;
                //        userDto.Gender = !string.IsNullOrWhiteSpace(data.sex) ? data.sex == "M" || data.sex == "m" ? 0 : 1 : null;
                //        userDto.BirthDate = dataBirthDate;
                //        userDto.Age = DateTimeUtility.CalculateAge(dataBirthDate);
                //        userDto.Agency = data.depend;
                //    }
                //    return new ResultResponse()
                //    {
                //        IsSuccess = true,
                //        Data = userDto
                //    };
                //}
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Data
                };
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await _userRepository.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultResponse> PatchUserAsync(User user)
        {
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
    }
}
