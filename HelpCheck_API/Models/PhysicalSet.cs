using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class PhysicalSet
    {
        [Key]
        public int ID { get; set; }
        public int? PhysicalID { get; set; }
        public string Descript { get; set; }
        public DateTime? PhysicalStartDateTime { get; set; }
        public DateTime? PhysicalEndDateTime { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}