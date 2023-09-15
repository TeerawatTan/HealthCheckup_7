using HelpCheck_API.Data;

namespace HelpCheck_API.Repositories.OtherInterfaceHospitals
{
    public class OtherInterfaceHospital : IOtherInterfaceHospital
    {
        private readonly ApplicationDbContext _context;
        public OtherInterfaceHospital(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
