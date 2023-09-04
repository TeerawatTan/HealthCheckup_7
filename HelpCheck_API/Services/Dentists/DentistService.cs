using System.Threading.Tasks;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Dentists;
using HelpCheck_API.Repositories.DentistChecks;
using HelpCheck_API.Repositories.Users;
using System;
using HelpCheck_API.Models;
using HelpCheck_API.Constants;
using System.Collections.Generic;
using System.Linq;
using HelpCheck_API.Repositories.MasterOralHealths;

namespace HelpCheck_API.Services.Dentists
{
    public class DentistService : IDentistService
    {
        private readonly IDentistCheckRepository _dentistCheckRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMasterOralHealthRepository _masterOralHealthRepository;

        public DentistService(IDentistCheckRepository dentistCheckRepository, IUserRepository userRepository, IMasterOralHealthRepository masterOralHealthRepository)
        {
            _dentistCheckRepository = dentistCheckRepository;
            _userRepository = userRepository;
            _masterOralHealthRepository = masterOralHealthRepository;
        }

        public async Task<ResultResponse> CreateDentistCheckAsync(AddDentistCheckDto addDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(addDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;

            try
            {
                DentistCheck model = new DentistCheck()
                {
                    MemberID = addDto.MemberID,
                    DoctorID = addDto.DoctorID,
                    Level = addDto.Level,
                    Detail = addDto.Detail,
                    CreateBy = userFullName
                };

                var resultCreated = await _dentistCheckRepository.CreateDentistCheckAsync(model);

                if (resultCreated.ID > 0)
                {
                    if (addDto.OralID.Count > 0)
                    {
                        List<DentistCheckOralHealth> list = new List<DentistCheckOralHealth>();

                        foreach (var oralId in addDto.OralID)
                        {
                            DentistCheckOralHealth oralModel = new DentistCheckOralHealth()
                            {
                                DentistCheckID = resultCreated.ID,
                                OralID = oralId
                            };

                            list.Add(oralModel);
                        }

                        if (list.Count > 0)
                        {
                            string status = await _dentistCheckRepository.CreateDentistCheckOralHealthsAsync(list);

                            if (status != Constant.STATUS_SUCCESS)
                            {
                                return new ResultResponse()
                                {
                                    IsSuccess = false,
                                    Data = status
                                };
                            }
                        }
                    }

                    return new ResultResponse()
                    {
                        IsSuccess = true,
                        Data = Constant.STATUS_SUCCESS
                    };
                }

                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_ERROR
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

        public async Task<ResultResponse> GetDentistCheckByMemberIDAsync(int memberId)
        {
            var data = await _dentistCheckRepository.GetDentistCheckByMemberIDAsync(memberId);

            GetDentistCheckDto getDto = new GetDentistCheckDto();

            if (data != null)
            {
                var checkOralData = await _dentistCheckRepository.GetDentistCheckByDentistIDAsync(data.ID);
                
                List<GetDropdownCodeAndNameDto> ddls = new List<GetDropdownCodeAndNameDto>();

                if (checkOralData.Any())
                {
                    ddls = checkOralData.Select(s => new GetDropdownCodeAndNameDto
                    {
                        ID = s.OralID,
                        Code = _masterOralHealthRepository.GetMasterOralHealthCodeByID(s.OralID),
                        Name = _masterOralHealthRepository.GetMasterOralHealthNameByID(s.OralID)
                    }).ToList();
                }

                getDto.ID = data.ID;
                getDto.MemberID = data.MemberID;
                getDto.DentistID = data.DoctorID;
                getDto.Level = data.Level;
                getDto.Detail = data.Detail;
                getDto.CreateDate = data.CreateDate;
                getDto.OralHealths = ddls;
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = getDto
            };
        }

        public async Task<ResultResponse> UpdateDentistCheckByMemberIDAsync(EditDentistCheckDto editDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(editDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;
            var data = await _dentistCheckRepository.GetDentistCheckByMemberIDAsync(editDto.MemberID);
            if (data is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            try
            {
                data.Level = editDto.Level;
                data.Detail = editDto.Detail;
                data.DoctorID = user.ID;
                data.Detail = editDto.Detail;
                data.UpdateBy = userFullName;

                string status = await _dentistCheckRepository.UpdateDentistCheckAsync(data);

                if (status != Constant.STATUS_SUCCESS)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = status
                    };
                }

                if (editDto.OralID.Count > 0)
                {
                    string resultRemoved = await _dentistCheckRepository.DeleteAllDentistCheckOralHealthAsync(data.ID);

                    if (resultRemoved != Constant.STATUS_SUCCESS)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = status
                        };
                    }

                    List<DentistCheckOralHealth> list = new List<DentistCheckOralHealth>();

                    foreach (var oralId in editDto.OralID)
                    {
                        DentistCheckOralHealth oralModel = new DentistCheckOralHealth()
                        {
                            DentistCheckID = data.ID,
                            OralID = oralId
                        };

                        list.Add(oralModel);
                    }

                    if (list.Count > 0)
                    {
                        string resultCreated = await _dentistCheckRepository.CreateDentistCheckOralHealthsAsync(list);

                        if (resultCreated != Constant.STATUS_SUCCESS)
                        {
                            return new ResultResponse()
                            {
                                IsSuccess = false,
                                Data = resultCreated
                            };
                        }
                    }
                }

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = Constant.STATUS_SUCCESS
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

        public async Task<bool> CheckExistsDentistCheckOralHealthsAsync(int oralId, int dentistCheckId)
        {
            var checkOral = await _dentistCheckRepository.GetDentistCheckOralHealthAsync(oralId, dentistCheckId);
            if (checkOral != null) return true;
            return false;
        }
    }
}