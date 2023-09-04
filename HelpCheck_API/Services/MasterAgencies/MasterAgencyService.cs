using System;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.MasterAgencies;

namespace HelpCheck_API.Services.MasterAgencies
{
    public class MasterAgencyService : IMasterAgencyService
    {
        private readonly IMasterAgencyRepository _masterAgencyRepository;
        public MasterAgencyService(IMasterAgencyRepository masterAgencyRepository)
        {
            _masterAgencyRepository = masterAgencyRepository;
        }

        public async Task<ResultResponse> GetMasterAgenciesAsync()
        {
            var agencies = await _masterAgencyRepository.GetMasterAgenciesAsync();

            var getDropdownDto = agencies.Select(s => new GetDropdownDto()
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

        public async Task<ResultResponse> CreateMasterAgencyAsync(AddMasterDataDto addMasterDataDto)
        {
            try
            {
                var masterAgency = new MasterAgency()
                {
                    Name = addMasterDataDto.Name
                };

                var status = await _masterAgencyRepository.CreateMasterAgencyAsync(masterAgency);

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

        public async Task<ResultResponse> UpdateMasterAgencyAsync(EditMasterDataDto editMasterDataDto)
        {
            var masterAgency = await _masterAgencyRepository.GetMasterAgencyByIDAsync(editMasterDataDto.ID);

            if (masterAgency is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            try
            {
                masterAgency.Name = editMasterDataDto.Name;

                var status = await _masterAgencyRepository.UpdateMasterAgencyAsync(masterAgency);

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

        public async Task<ResultResponse> DeleteMasterAgencyAsync(int id)
        {
            var masterAgency = await _masterAgencyRepository.GetMasterAgencyByIDAsync(id);

            if (masterAgency is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            try
            {
                var status = await _masterAgencyRepository.DeleteMasterAgencyAsync(masterAgency);

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