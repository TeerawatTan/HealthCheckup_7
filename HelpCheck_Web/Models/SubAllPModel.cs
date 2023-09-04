using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class SubAllPModel
    {
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int totalItems { get; set; }
        public int totalPages { get; set; }
        public List<AllPatientModel> Items { get; set; }
    }
}
