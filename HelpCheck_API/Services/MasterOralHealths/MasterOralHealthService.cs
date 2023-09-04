using System;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.MasterOralHealths;

namespace HelpCheck_API.Services.MasterOralHealths
{
    public class MasterOralHealthService : IMasterOralHealthService
    {
        private readonly IMasterOralHealthRepository _masterOralHealthRepository;
        public MasterOralHealthService(IMasterOralHealthRepository masterOralHealthRepository)
        {
            _masterOralHealthRepository = masterOralHealthRepository;
        }

        public async Task<ResultResponse> GetMasterOralHealthsAsync()
        {
            var OralHealths = await _masterOralHealthRepository.GetMasterOralHealthsAsync();

            var getDropdownDto = OralHealths.Select(s => new GetDropdownCodeAndNameDto()
            {
                ID = s.ID,
                Code = s.Code,
                Name = s.Name
            });
            
            return new ResultResponse()
            {
                IsSuccess = true,
                Data = getDropdownDto
            };
        }
        
        public async Task<ResultResponse> CreateMasterOralHealthAsync(AddMasterDataCodeAndNameDto addMasterDataDto)
        {
            try
            {
                var masterOralHealth = new MasterOralHealth()
                {
                    Name = addMasterDataDto.Name,
                    Code = addMasterDataDto.Code
                    
                };

                var status = await _masterOralHealthRepository.CreateMasterOralHealthAsync(masterOralHealth);

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

        public async Task<ResultResponse> UpdateMasterOralHealthAsync(EditMasterCodeAndNameDto editMasterDataDto)
        {
            var masterOralHealth = await _masterOralHealthRepository.GetMasterOralHealthByIDAsync(editMasterDataDto.ID);

            if (masterOralHealth is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            try
            {
                masterOralHealth.Name = editMasterDataDto.Name;
                masterOralHealth.Code = editMasterDataDto.Code;

                var status = await _masterOralHealthRepository.UpdateMasterOralHealthAsync(masterOralHealth);

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

        public async Task<ResultResponse> DeleteMasterOralHealthAsync(int id)
        {
            var masterOralHealth = await _masterOralHealthRepository.GetMasterOralHealthByIDAsync(id);

            if (masterOralHealth is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }
            
            try
            {
                var status = await _masterOralHealthRepository.DeleteMasterOralHealthAsync(masterOralHealth);

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