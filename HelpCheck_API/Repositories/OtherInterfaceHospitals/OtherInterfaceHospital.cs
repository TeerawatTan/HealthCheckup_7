using HelpCheck_API.Data;
using HelpCheck_API.Dtos.Reports;
using System.Collections.Generic;
using System.Linq;

namespace HelpCheck_API.Repositories.OtherInterfaceHospitals
{
    public class OtherInterfaceHospital : IOtherInterfaceHospital
    {
        private readonly ApplicationDbContext _context;
        public OtherInterfaceHospital(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<CheckResultDetailFromPMKDto> GetDetailResultFromHospitals()
        {
            return _context.DetailResultFromHospitals.ToList();
        }
    }
}
