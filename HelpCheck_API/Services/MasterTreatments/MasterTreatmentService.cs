using System;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.MasterTreatments;

namespace HelpCheck_API.Services.MasterTreatments
{
    public class MasterTreatmentService : IMasterTreatmentService
    {
        private readonly IMasterTreatmentRepository _masterTreatmentRepository;
        public MasterTreatmentService(IMasterTreatmentRepository masterTreatmentRepository)
        {
            _masterTreatmentRepository = masterTreatmentRepository;
        }

        public async Task<ResultResponse> GetMasterTreatmentsAsync()
        {
            var Treatments = await _masterTreatmentRepository.GetMasterTreatmentsAsync();

            var getDropdownDto = Treatments.Select(s => new GetDropdownDto()
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
        
        public async Task<ResultResponse> CreateMasterTreatmentAsync(AddMasterDataDto addMasterDataDto)
        {
            try
            {
                var masterTreatment = new MasterTreatment()
                {
                    Name = addMasterDataDto.Name
                };

                var status = await _masterTreatmentRepository.CreateMasterTreatmentAsync(masterTreatment);

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

        public async Task<ResultResponse> UpdateMasterTreatmentAsync(EditMasterDataDto editMasterDataDto)
        {
            var masterTreatment = await _masterTreatmentRepository.GetMasterTreatmentByIDAsync(editMasterDataDto.ID);

            if (masterTreatment is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            try
            {
                masterTreatment.Name = editMasterDataDto.Name;

                var status = await _masterTreatmentRepository.UpdateMasterTreatmentAsync(masterTreatment);

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

        public async Task<ResultResponse> DeleteMasterTreatmentAsync(int id)
        {
            var masterTreatment = await _masterTreatmentRepository.GetMasterTreatmentByIDAsync(id);

            if (masterTreatment is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }
            
            try
            {
                var status = await _masterTreatmentRepository.DeleteMasterTreatmentAsync(masterTreatment);

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