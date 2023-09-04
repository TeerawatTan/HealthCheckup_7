using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class PhysicalExaminationMaster
    {
        [Key]
        public int ID { get; set; }
        public int? PhysicalID { get; set; }
        public int? PhChoiceID { get; set; }
        public int? PhysicalAgeStart { get; set; }
        public int? PhysicalAgeEnd { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}