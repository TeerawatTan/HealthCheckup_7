using System.Linq;
using System.Threading.Tasks;
using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Physicals;
using HelpCheck_API.Repositories.PhysicalExaminationMasters;

namespace HelpCheck_API.Services.Physicals
{
    public class PhysicalService : IPhysicalService
    {
        private readonly IPhysicalRepository _physicalRepository;

        public PhysicalService(IPhysicalRepository physicalRepository)
        {
            _physicalRepository = physicalRepository;
        }

        public async Task<ResultResponse> GetPhysicalDropdownListAsync()
        {
            var physicals = await _physicalRepository.GetPhysicalDropdownListAsync();

            var getDropdownDto = physicals.Select(s => new GetDropdownDto()
            {
                ID = s.ID,
                Name = s.Descript
            });

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = getDropdownDto
            };
        }

        public async Task<ResultResponse> GetPhysicalExaminationMasterListByPhysicalSetIDAsync(int physicalSetId)
        {
            var physicalExaminationMasters = await _physicalRepository.GetPhysicalExaminationMasterListByPhysicalSetIDAsync(physicalSetId);

            if (physicalExaminationMasters is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            var datas = physicalExaminationMasters.Select(s => new GetPhysicalExaminationMasterDto
            {
                ID = s.ID,
                DescriptTh = s.DescriptTh,
                DescriptEn = s.DescriptEn,
                UnitTh = s.UnitTh,
                UnitEn = s.UnitEn,
                Image = s.Image,
                BgColor = s.BgColor,
                Value = ""
            });

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = datas
            };
        }
    }
}