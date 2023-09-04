using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class DentistDto
    {
        public int MemberId { get; set; }
        public int DoctorId { get; set; }
        public int? Level { get; set; }
        public string Detail { get; set; }
        public int[] OralID { get; set; }
    }

    public class GetDentistCheckDto
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int DentistID { get; set; }
        public int? Level { get; set; }
        public string Detail { get; set; }
        public DateTime? CreateDate { get; set; }
        public List<Oral> oralHealths { get; set; }

    }

    public class Oral
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
