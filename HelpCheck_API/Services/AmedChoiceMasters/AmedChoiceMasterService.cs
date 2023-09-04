using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.AmedChoiceMasters;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.AmedChoiceMasters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.AmedChoiceMasters
{
    public class AmedChoiceMasterService : BaseService, IAmedChoiceMasterService
    {
        private readonly IAmedChoiceMasterRepository _amedChoiceMasterRepository;

        public AmedChoiceMasterService(IAmedChoiceMasterRepository amedChoiceMasterRepository)
        {
            _amedChoiceMasterRepository = amedChoiceMasterRepository;
        }

        public async Task<ResultResponse> CreateAmedChoiceMasterAsync(AddAmedChoiceMasterDto data)
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

                var model = new AmedChoiceMaster()
                {
                    ChoiceNum = data.ChoiceNum,
                    ChoiceName = data.ChoiceName,
                    CreatedBy = await GetUserFullNameByAccessTokenAsync(data.AccessToken),
                    Score = data.Score,
                    IsActive = data.IsUse ?? true
                };

                var status = await _amedChoiceMasterRepository.CreateAmedChoiceMasterAsync(model);

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

        public async Task<ResultResponse> DeleteAmedChoiceMasterByIDAsync(int id, string accessToken)
        {
            var amedChoiceMaster = await _amedChoiceMasterRepository.GetAmedChoiceMasterByIDAsync(id);

            if (amedChoiceMaster is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }
            amedChoiceMaster.UpdatedBy = await GetUserFullNameByAccessTokenAsync(accessToken);
            amedChoiceMaster.IsActive = false;

            var status = await _amedChoiceMasterRepository.UpdateAmedChoiceMasterAsync(amedChoiceMaster);

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

        public async Task<ResultResponse> GetAmedChoiceMasterAsync()
        {
            var data = await _amedChoiceMasterRepository.GetAmedChoiceMastersAsync();

            var amedChoiceMasters = data.Select(s => new GetAmedChoiceMasterDto
            {
                ID = s.ChoiceID,
                ChoiceNum = s.ChoiceNum,
                ChoiceName = s.ChoiceName,
                Score = s.Score,
                IsUse = s.IsActive ?? true
            }).ToList();

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = amedChoiceMasters
            };
        }

        public async Task<ResultResponse> GetAmedChoiceMasterByIDAsync(int id)
        {
            var data = await _amedChoiceMasterRepository.GetAmedChoiceMasterByIDAsync(id);

            if (data is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            var amedChoiceMaster = new GetAmedChoiceMasterDto
            {
                ID = data.ChoiceID,
                ChoiceNum = data.ChoiceNum,
                ChoiceName = data.ChoiceName,
                Score = data.Score,
                IsUse = data.IsActive ?? true
            };

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = amedChoiceMaster
            };
        }

        public async Task<ResultResponse> UpdateAmedChoiceMasterByIDAsync(EditAmedChoiceMasterDto data)
        {
            var amedChoiceMaster = await _amedChoiceMasterRepository.GetAmedChoiceMasterByIDAsync(data.ID);

            if (amedChoiceMaster is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            amedChoiceMaster.ChoiceNum = data.ChoiceNum;
            amedChoiceMaster.ChoiceName = data.ChoiceName;
            amedChoiceMaster.UpdatedBy = await GetUserFullNameByAccessTokenAsync(data.AccessToken);
            amedChoiceMaster.Score = data.Score;
            amedChoiceMaster.IsActive = data.IsUse ?? true;

            var status = await _amedChoiceMasterRepository.UpdateAmedChoiceMasterAsync(amedChoiceMaster);

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
