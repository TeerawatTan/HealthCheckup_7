using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Dtos.Patients
{
    public class GetXRayResultDto
    {
        public string id { get; set; }
        public string hn { get; set; }
        public string result_date { get; set; }
        public int VN { get; set; }
        public string xray_group { get; set; }
        public string xray_code { get; set; }
        public string result { get; set; }
    }

    public class DataXRayResult
    {
        public List<GetXRayResultDto> Data { get; set; }
    }
}
