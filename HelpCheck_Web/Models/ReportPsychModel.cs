using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class ReportPsychModel
    {
        public string memberID { get; set; }
        public string titleName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string hn { get; set; }
        public string idCard { get; set; }
        public string workPlaceName { get; set; }
        public string jobTypeName { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string treatmentName { get; set; }
        public string question2QOne { get; set; }
        public string question2QTwo { get; set; }
        public int? question9QOne { get; set; }
        public int? question9QTwo { get; set; }
        public int? question9QThree { get; set; }
        public int? question9QFour { get; set; }
        public int? question9QFive { get; set; }
        public int? question9QSix { get; set; }
        public int? question9QSeven { get; set; }
        public int? question9QEight { get; set; }
        public int? question9QNine { get; set; }
        public int? sumQuestion9Q { get; set; }
    }
}
