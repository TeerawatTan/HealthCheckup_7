using HelpCheck_API.Dtos.Patients;
using System.Collections.Generic;

namespace HelpCheck_API.Repositories.OtherInterfaces
{
    public interface IOtherInterfaceRepository
    {
        List<GetXRayResultDto> GetXRayResults();
        List<GetBloodResultDto> GetBloodResults();
        List<GetLabSmearDetailDto> GetLabSmearDetails();
    }
}