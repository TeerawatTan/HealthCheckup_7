using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Dtos.Patients
{
    public class AddPhysicalDto
    {
        internal string AccessToken { get; set; }
        public int MemberID { get; set; }
        public int PhysicalSetID { get; set; }
        public List<PhysicalDetailDto> PhysicalDetails { get; set; }
    }

    public class PhysicalDetailDto
    {
        public int PhysicalChoiceID { get; set; }
        public string Value { get; set; }
    }
}
