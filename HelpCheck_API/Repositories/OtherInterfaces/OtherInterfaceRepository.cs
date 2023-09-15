using HelpCheck_API.Data;
using HelpCheck_API.Dtos.Patients;
using System.Collections.Generic;
using System.Linq;

namespace HelpCheck_API.Repositories.OtherInterfaces
{
    public class OtherInterfaceRepository : IOtherInterfaceRepository
    {
        private readonly ApplicationDbContext _context;

        public OtherInterfaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<GetBloodResultDto> GetBloodResults()
        {
            return _context.BloodResults.ToList();
        }

        public List<GetLabSmearDetailDto> GetLabSmearDetails()
        {
            return _context.LabSmearDetails.ToList();
        }

        public List<GetXRayResultDto> GetXRayResults()
        {
            return _context.XRayResults.ToList();
        }
    }
}