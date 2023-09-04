using System.Threading.Tasks;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Doctors;
using HelpCheck_API.Repositories.DoctorChecks;
using HelpCheck_API.Repositories.Users;
using System;
using HelpCheck_API.Models;
using HelpCheck_API.Constants;

namespace HelpCheck_API.Services.Doctors
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorCheckRepository _doctorCheckRepository;
        private readonly IUserRepository _userRepository;

        public DoctorService(IDoctorCheckRepository doctorCheckRepository, IUserRepository userRepository)
        {
            _doctorCheckRepository = doctorCheckRepository;
            _userRepository = userRepository;
        }

        public async Task<ResultResponse> CreateDoctorCheckAsync(AddDoctorCheckDto addDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(addDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;

            try
            {
                DoctorCheck model = new DoctorCheck()
                {
                    MemberID = addDto.MemberID,
                    DoctorID = addDto.DoctorID,
                    IsResult = addDto.IsResult,
                    IsBcc = addDto.IsBcc,
                    Detail = addDto.Detail,
                    IsInsideCheck = addDto.IsInsideCheck,
                    InsideDetail = addDto.InsideDetail,
                    IsPapSmearCheck = addDto.IsPapSmearCheck,
                    CreateBy = userFullName
                };

                string status = await _doctorCheckRepository.CreateDoctorCheckAsync(model);

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

        public async Task<ResultResponse> GetDoctorCheckByMemberIDAsync(int memberId)
        {
            var data = await _doctorCheckRepository.GetDoctorCheckByMemberIDAsync(memberId);

            GetDoctorCheckDto getDto = new GetDoctorCheckDto();

            if (data != null)
            {
                getDto.ID = data.ID;
                getDto.MemberID = data.MemberID;
                getDto.DoctorID = data.DoctorID;
                getDto.IsResult = data.IsResult;
                getDto.IsBcc = data.IsBcc;
                getDto.Detail = data.Detail;
                getDto.IsInsideCheck = data.IsInsideCheck;
                getDto.InsideDetail = data.InsideDetail;
                getDto.IsPapSmearCheck = data.IsPapSmearCheck;
                getDto.CreateDate = data.CreateDate;
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = getDto
            };
        }

        public async Task<ResultResponse> UpdateDoctorCheckByMemberIDAsync(EditDoctorCheckDto editDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(editDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;
            var data = await _doctorCheckRepository.GetDoctorCheckByMemberIDAsync(editDto.MemberID);
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
                data.IsResult = editDto.IsResult;
                data.IsBcc = editDto.IsBcc;
                data.Detail = editDto.Detail;
                data.IsInsideCheck = editDto.IsInsideCheck;
                data.InsideDetail = editDto.InsideDetail;
                data.IsPapSmearCheck = editDto.IsPapSmearCheck;
                data.UpdateBy = userFullName;

                string status = await _doctorCheckRepository.UpdateDoctorCheckAsync(data);

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