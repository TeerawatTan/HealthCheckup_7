using System;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.MasterWorkPlaces;

namespace HelpCheck_API.Services.MasterWorkPlaces
{
    public class MasterWorkPlaceService : IMasterWorkPlaceService
    {
        private readonly IMasterWorkPlaceRepository _masterWorkPlaceRepository;
        public MasterWorkPlaceService(IMasterWorkPlaceRepository masterWorkPlaceRepository)
        {
            _masterWorkPlaceRepository = masterWorkPlaceRepository;
        }

        public async Task<ResultResponse> GetMasterWorkPlacesAsync()
        {
            var workPlaces = await _masterWorkPlaceRepository.GetMasterWorkPlacesAsync();

            var getDropdownDto = workPlaces.Select(s => new GetDropdownDto()
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
        
        public async Task<ResultResponse> CreateMasterWorkPlaceAsync(AddMasterDataDto addMasterDataDto)
        {
            try
            {
                var masterWorkPlace = new MasterWorkPlace()
                {
                    Name = addMasterDataDto.Name
                };

                var status = await _masterWorkPlaceRepository.CreateMasterWorkPlaceAsync(masterWorkPlace);

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

        public async Task<ResultResponse> UpdateMasterWorkPlaceAsync(EditMasterDataDto editMasterDataDto)
        {
            var masterWorkPlace = await _masterWorkPlaceRepository.GetMasterWorkPlaceByIDAsync(editMasterDataDto.ID);

            if (masterWorkPlace is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            try
            {
                masterWorkPlace.Name = editMasterDataDto.Name;

                var status = await _masterWorkPlaceRepository.UpdateMasterWorkPlaceAsync(masterWorkPlace);

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

        public async Task<ResultResponse> DeleteMasterWorkPlaceAsync(int id)
        {
            var masterWorkPlace = await _masterWorkPlaceRepository.GetMasterWorkPlaceByIDAsync(id);

            if (masterWorkPlace is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }
            
            try
            {
                var status = await _masterWorkPlaceRepository.DeleteMasterWorkPlaceAsync(masterWorkPlace);

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