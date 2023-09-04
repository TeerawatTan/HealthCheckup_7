using System.Threading.Tasks;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.AmedQuestionMasters;
using System;
using HelpCheck_API.Models;
using HelpCheck_API.Constants;
using HelpCheck_API.Repositories.AmedQuestionMasters;
using System.Linq;

namespace HelpCheck_API.Services.AmedQuestionMasters
{
    public class AmedQuestionMasterService : BaseService, IAmedQuestionMasterService
    {
        private readonly IAmedQuestionMasterRepository _amedQuestionMasterRepository;
        public AmedQuestionMasterService(IAmedQuestionMasterRepository amedQuestionMasterRepository) : base()
        {
            _amedQuestionMasterRepository = amedQuestionMasterRepository;
        }

        public async Task<ResultResponse> GetAmedQuestionMasterMapChoicesAsync()
        {
            var data = await _amedQuestionMasterRepository.GetAmedQuestionMastersAsync();

            var amedQuestionMasters = data.Select(s => new GetAmedQuestionMasterDto()
            {
                ID = s.QuestionID,
                QuestionNum = s.QuestionNum,
                QuestionName = s.QuestionName,
                QuestionPeriod = s.QuestionPeriod,
                CreatedBy = s.CreatedBy,
                CreatedDate = s.CreatedDate,
                IsUse = s.IsActive ?? true
            });

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = amedQuestionMasters
            };
        }

        public async Task<ResultResponse> GetAmedQuestionMasterMapChoicesByQuestionIDAsync(int questionId)
        {
            var data = await _amedQuestionMasterRepository.GetAmedQuestionMasterByIDAsync(questionId);

            if (data is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            var getAmedQuestionMasterDto = new GetAmedQuestionMasterDto();
            getAmedQuestionMasterDto.ID = data.QuestionID;
            getAmedQuestionMasterDto.QuestionNum = data.QuestionNum;
            getAmedQuestionMasterDto.QuestionName = data.QuestionName;
            getAmedQuestionMasterDto.QuestionPeriod = data.QuestionPeriod;
            getAmedQuestionMasterDto.IsUse = data.IsActive ?? true;

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = getAmedQuestionMasterDto
            };
        }

        public async Task<ResultResponse> CreateAmedQuestionMasterAsync(AddAmedQuestionMasterDto data)
        {
            try
            {
                if (data is null)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = Constant.STATUS_INVALID_REQUEST_DATA
                    };
                }

                var model = new AmedQuestionMaster()
                {
                    QuestionNum = data.QuestionNum,
                    QuestionName = data.QuestionName,
                    CreatedBy = await GetUserFullNameByAccessTokenAsync(data.AccessToken),
                    IsActive = data.IsUse ?? true
                };

                var status = await _amedQuestionMasterRepository.CreateAmedQuestionMasterAsync(model);

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

        public async Task<ResultResponse> DeleteAmedQuestionMasterByIDAsync(int id, string accessToken)
        {
            var amedQuestionMaster = await _amedQuestionMasterRepository.GetAmedQuestionMasterByIDAsync(id);

            if (amedQuestionMaster is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            amedQuestionMaster.IsActive = false;
            amedQuestionMaster.UpdatedBy = await GetUserFullNameByAccessTokenAsync(accessToken);

            var status = await _amedQuestionMasterRepository.UpdateAmedQuestionMasterAsync(amedQuestionMaster);

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

        public async Task<ResultResponse> UpdateAmedQuestionMasterByIDAsync(EditAmedQuestionMasterDto data)
        {
            var amedQuestionMaster = await _amedQuestionMasterRepository.GetAmedQuestionMasterByIDAsync(data.ID);

            if (amedQuestionMaster is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            amedQuestionMaster.QuestionNum = data.QuestionNum;
            amedQuestionMaster.QuestionName = data.QuestionName;
            amedQuestionMaster.QuestionPeriod = data.QuestionPeriod;
            amedQuestionMaster.UpdatedBy = await GetUserFullNameByAccessTokenAsync(data.AccessToken);
            amedQuestionMaster.IsActive = data.IsUse ?? true;

            var status = await _amedQuestionMasterRepository.UpdateAmedQuestionMasterAsync(amedQuestionMaster);

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
    }
}