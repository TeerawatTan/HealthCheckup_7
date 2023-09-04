using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Models
{
    public class ViewAnswerPhysical
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string DescriptTh { get; set; }
        public string DescriptEn { get; set; }
        public string AnsphyKeyIn { get; set; }
        public string UnitTh { get; set; }
        public string UnitEn { get; set; }
    }
}
