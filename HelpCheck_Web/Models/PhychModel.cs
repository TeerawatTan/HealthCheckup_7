using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class PhychModel
    {
        public int MemberId { get; set; }
        public int DoctorId { get; set; }
        public string Detail { get; set; }
    }
    public class AddPhychModel
    {
        public int MemberID { get; set; }
        public int DoctorID { get; set; }
        public string Detail { get; set; }
    }
}
