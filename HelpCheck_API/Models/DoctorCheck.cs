using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class DoctorCheck
    {
        [Key]
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int DoctorID { get; set; }
        public int? IsResult { get; set; }
        public bool? IsBcc { get; set; }    // �������ҹ� 
        public bool? IsInsideCheck { get; set; } // ��Ǩ����
        public string InsideDetail { get; set; }
        public bool? IsPapSmearCheck { get; set; } // ����移ҡ���١
        public string Detail { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}