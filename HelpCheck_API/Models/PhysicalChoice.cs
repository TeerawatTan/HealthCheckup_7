using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class PhysicalChoice
    {
        [Key]
        public int ID { get; set; }
        public string DescriptTh { get; set; }
        public string DescriptEn { get; set; }
        public string UnitTh { get; set; }
        public string UnitEn { get; set; }
        public string Image { get; set; }
        public string BgColor { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}