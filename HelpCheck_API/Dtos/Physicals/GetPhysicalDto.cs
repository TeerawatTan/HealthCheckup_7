using System;

namespace HelpCheck_API.Dtos.Physicals
{
    public class GetPhysicalExaminationMasterDto
    {
        public int ID { get; set; }
        public string DescriptTh { get; set; }
        public string DescriptEn { get; set; }
        public string Value { get; set; }
        public string UnitTh { get; set; }
        public string UnitEn { get; set; }
        public string Image { get; set; }
        public string BgColor { get; set; }
        internal DateTime? CreatedDate { get; set; }
    }

    public class GetPhysicalDetailDto
    {
        public int MemberID { get; set; }
        public int PhysicalChoiceID { get; set; }
        //public bool IsPhysicalCheck { get; set; }
        //public bool IsWeightCheck { get; set; }
        //public bool IsHeightCheck { get; set; }
        //public bool IsBmiCheck { get; set; }
        //public bool IsWaistlineCheck { get; set; }
        //public bool IsbloodPressureCheck { get; set; }
    }
}