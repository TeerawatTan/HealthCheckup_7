using HelpCheck_API.Data;
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

    }
}