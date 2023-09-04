using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Psychiatrists;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.PsychiatristChecks;
using HelpCheck_API.Repositories.Users;
using System;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.PsychiatristChecks
{
    public class PsychiatristCheckService : IPsychiatristCheckService
    {
        private readonly IPsychiatristCheckRepository _psychiatristCheckRepository;
        private readonly IUserRepository _userRepository;

        public PsychiatristCheckService(IPsychiatristCheckRepository psychiatristCheckRepository, IUserRepository userRepository)
        {
            _psychiatristCheckRepository = psychiatristCheckRepository;
            _userRepository = userRepository;
        }

        public async Task<ResultResponse> CreatePsychiatristCheckAsync(AddPsychiatristCheckDto addDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(addDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;

            try
            {
                PsychiatristCheck model = new PsychiatristCheck()
                {
                    MemberID = addDto.MemberID,
                    DoctorID = addDto.DoctorID,
                    Detail = addDto.Detail,
                    CreateBy = userFullName
                };

                string status = await _psychiatristCheckRepository.CreatePsychiatristCheckAsync(model);

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
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }

        public async Task<ResultResponse> GetPsychiatristCheckByMemberIDAsync(int memberId)
        {
            var data = await _psychiatristCheckRepository.GetPsychiatristCheckByMemberIDAsync(memberId);

            GetPsychiatristDto getDto = new GetPsychiatristDto();

            if (data != null)
            {
                getDto.ID = data.ID;
                getDto.MemberID = data.MemberID;
                getDto.DoctorID = data.DoctorID;
                getDto.Detail = data.Detail;
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = getDto
            };
        }

        public async Task<ResultResponse> UpdatePsychiatristCheckByMemberIDAsync(EditPsychiatristCheckDto editDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(editDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;
            var data = await _psychiatristCheckRepository.GetPsychiatristCheckByMemberIDAsync(editDto.MemberID);
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
                data.DoctorID = user.ID;
                data.Detail = editDto.Detail;
                data.UpdateBy = userFullName;

                string status = await _psychiatristCheckRepository.UpdatePsychiatristCheckAsync(data);

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
