using HelpCheck_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.MasterWorkPlaces
{
    public interface IMasterWorkPlaceRepository
    {
        Task<List<MasterWorkPlace>> GetMasterWorkPlacesAsync();
        MasterWorkPlace GetMasterWorkPlaceByID(int id);
        string GetMasterWorkPlaceNameByID(int id);
        Task<MasterWorkPlace> GetMasterWorkPlaceByIDAsync(int id);
        Task<string> CreateMasterWorkPlaceAsync(MasterWorkPlace masterWorkPlace);
        Task<string> UpdateMasterWorkPlaceAsync(MasterWorkPlace masterWorkPlace);
        Task<string> DeleteMasterWorkPlaceAsync(MasterWorkPlace masterWorkPlace);
    }
}
