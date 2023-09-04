using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.AmedChoiceMasters
{
    public interface IAmedChoiceMasterRepository
    {
        Task<IEnumerable<AmedChoiceMaster>> GetAmedChoiceMastersAsync();
        Task<AmedChoiceMaster> GetAmedChoiceMasterByIDAsync(int id);
        Task<string> CreateAmedChoiceMasterAsync(AmedChoiceMaster data);
        Task<string> UpdateAmedChoiceMasterAsync(AmedChoiceMaster data);
        Task<string> DeleteAmedChoiceMasterAsync(AmedChoiceMaster data);
    }
}