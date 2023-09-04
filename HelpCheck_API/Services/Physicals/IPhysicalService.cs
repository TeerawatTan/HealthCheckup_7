using System.Threading.Tasks;
using HelpCheck_API.Dtos;

namespace HelpCheck_API.Services.Physicals
{
    public interface IPhysicalService
    {
        Task<ResultResponse> GetPhysicalDropdownListAsync();
        Task<ResultResponse> GetPhysicalExaminationMasterListByPhysicalSetIDAsync(int physicalSetId);
    }
}