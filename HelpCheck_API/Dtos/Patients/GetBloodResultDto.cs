using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Dtos.Patients
{
    public class GetBloodResultDto
    {
        public int vn { get; set; }
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

    public class DataBloodResult
    {
        public List<GetBloodResultDto> Data { get; set; }
    }
}
