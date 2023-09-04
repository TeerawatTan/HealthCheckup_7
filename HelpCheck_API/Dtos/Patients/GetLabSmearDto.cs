using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Dtos.Patients
{
    public class GetLabSmearDetailDto
    {
        public int VN { get; set; }
        public string HN { get; set; }
        public string LABCODE { get; set; }
        public string RESULT_DATE { get; set; }
        public string RESULT { get; set; }
    }

    public class GetLabSmearDto
    {
        public List<GetLabSmearDetailDto> Data { get; set; }
    }


}
