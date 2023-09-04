using System;

namespace HelpCheck_API.Dtos.Doctors
{
    public class EditDoctorCheckDto
    {
        internal string AccessToken { get; set; }
        public int MemberID { get; set; }
        public int? IsResult { get; set; }
        public bool? IsBcc { get; set; }    // �������ҹ� 
        public string Detail { get; set; }
        public bool? IsInsideCheck { get; set; } // ��Ǩ����
        public string InsideDetail { get; set; }
        public bool? IsPapSmearCheck { get; set; } // ����移ҡ���١
    }
}