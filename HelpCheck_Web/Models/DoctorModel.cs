using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class AddDoctorModel
    {
        public int MemberID { get; set; }
        public int DoctorID { get; set; }
        public int IsResult { get; set; }
        public bool IsBcc { get; set; }
        public string Detail { get; set; }
        public bool IsInsideCheck { get; set; }
        public string InsideDetail { get; set; }
        public bool IsPapSmearCheck { get; set; }
    }

    public class DoctorDto
    {
        public int MemberId { get; set; }
        public int DoctorId { get; set; }
        public int IsResult { get; set; }
        public bool IsBcc { get; set; }
        public string Detail { get; set; }
        public bool IsInsideCheck { get; set; }
        public string InsideDetail { get; set; }
        public bool IsPapSmearCheck { get; set; }
        public DateTime? CreateDate { get; set; }
    }

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
    }

    public class GetDoctorCheckDto
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int DoctorID { get; set; }
        public int IsResult { get; set; }
        public bool IsBcc { get; set; }
        public string Detail { get; set; }
        public bool IsInsideCheck { get; set; }
        public string InsideDetail { get; set; }
        public bool IsPapSmearCheck { get; set; }
    }

    public class RootX
    {
        public List<XrayModel> data { get; set; }
    }

    public class XrayModel
    {
        public string id { get; set; }
        public string hn { get; set; }
        public string result_date { get; set; }
        public string vn { get; set; }
        public string xray_group { get; set; }
        public string xray_code { get; set; }
        public string result { get; set; }
    }
    public class RootB
    {
        public List<BloodModel> data { get; set; }
    }

    public class BloodModel
    {
        public string vn { get; set; }
        public string result_date { get; set; }
        public string doctorname { get; set; }
        public string code { get; set; }
        public string group_lab { get; set; }
        public string labname { get; set; }
        public string labcode_detial { get; set; }
        public string result_name { get; set; }
        public string data_type { get; set; }
        public string result { get; set; }
        public string unit_text { get; set; }
        public string refference_range { get; set; }
        public string clinic { get; set; }
        public string check_nomal { get; set; }
        public string check_critical { get; set; }
    }
    public class RootP
    {
        public List<PrepModel> data { get; set; }
    }

    public class PrepModel
    {
        public string vn { get; set; }
        public string hn { get; set; }
        public string labcode { get; set; }
        public string resulT_DATE { get; set; }
        public string result { get; set; }

    }
}
