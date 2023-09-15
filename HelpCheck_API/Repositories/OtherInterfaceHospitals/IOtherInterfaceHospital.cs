using HelpCheck_API.Dtos.Reports;
using System.Collections.Generic;

namespace HelpCheck_API.Repositories.OtherInterfaceHospitals
{
    public interface IOtherInterfaceHospital
    {
        List<CheckResultDetailFromPMKDto> GetDetailResultFromHospitals();
    }

}
