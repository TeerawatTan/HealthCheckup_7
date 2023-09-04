using System;
using System.Collections.Generic;

namespace HelpCheck_Web.Models
{
    public class PhysicalModel
    {
        public int PhysicalChoiceID { get; set; }
        public string Value { get; set; }
    }

    public class PhysicaldetailModel
    {
        public int id { get; set; }
        public string descriptTh { get; set; }
        public string descriptEn { get; set; }
        public string value { get; set; }
        public string unitTh { get; set; }
        public string unitEn { get; set; }
        public string image { get; set; }
        public string bgColor { get; set; }
    }

    public class CreatePhysicalDto
    {
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
