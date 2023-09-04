using System.Collections.Generic;
using System.Threading.Tasks;
using HelpCheck_API.Dtos.Physicals;
using HelpCheck_API.Models;

namespace HelpCheck_API.Repositories.PhysicalExaminationMasters
{
    public interface IPhysicalRepository
    {
        Task<List<PhysicalSet>> GetPhysicalDropdownListAsync();
        Task<List<GetPhysicalExaminationMasterDto>> GetPhysicalExaminationMasterListByPhysicalSetIDAsync(int physicalSetId);
        Task<List<GetPhysicalExaminationMasterDto>> GetPhysicalExaminationMasterListByPhysicalIDAsync(int physicalId);
        Task<List<GetPhysicalExaminationMasterDto>> GetPhysicalAnswerListByMemberIDAsync(int memberId);
    }
}