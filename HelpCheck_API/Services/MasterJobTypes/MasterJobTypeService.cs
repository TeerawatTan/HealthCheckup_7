using System;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.MasterJobTypes;

namespace HelpCheck_API.Services.MasterJobTypes
{
    public class MasterJobTypeService : IMasterJobTypeService
    {
        private readonly IMasterJobTypeRepository _masterJobTypeRepository;
        public MasterJobTypeService(IMasterJobTypeRepository masterJobTypeRepository)
        {
            _masterJobTypeRepository = masterJobTypeRepository;
        }

        public async Task<ResultResponse> GetMasterJobTypesAsync()
        {
            var jobTypes = await _masterJobTypeRepository.GetMasterJobTypesAsync();

            var getDropdownDto = jobTypes.Select(s => new GetDropdownDto()
            {
                ID = s.ID,
                Name = s.Name
            });

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = getDropdownDto
            };
        }

        public async Task<ResultResponse> CreateMasterJobTypeAsync(AddMasterDataDto addMasterDataDto)
        {
            try
            {
                var masterJobType = new MasterJobType()
                {
                    Name = addMasterDataDto.Name
                };

                var status = await _masterJobTypeRepository.CreateMasterJobTypeAsync(masterJobType);

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

        public async Task<ResultResponse> UpdateMasterJobTypeAsync(EditMasterDataDto editMasterDataDto)
        {
            var masterJobType = await _masterJobTypeRepository.GetMasterJobTypeByIDAsync(editMasterDataDto.ID);

            if (masterJobType is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            try
            {
                masterJobType.Name = editMasterDataDto.Name;

                var status = await _masterJobTypeRepository.UpdateMasterJobTypeAsync(masterJobType);

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

        public async Task<ResultResponse> DeleteMasterJobTypeAsync(int id)
        {
            var masterJobType = await _masterJobTypeRepository.GetMasterJobTypeByIDAsync(id);

            if (masterJobType is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }
            
            try
            {
                var status = await _masterJobTypeRepository.DeleteMasterJobTypeAsync(masterJobType);

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