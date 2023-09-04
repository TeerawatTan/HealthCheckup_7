using System;

namespace HelpCheck_API.Dtos.Doctors
{
    public class AddDoctorCheckDto
    {
        internal string AccessToken { get; set; }
        public int MemberID { get; set; }
        public int DoctorID { get; set; }
        public int? IsResult { get; set; }
        public bool? IsBcc { get; set; }    // มะเร็งเต้านม 
        public string Detail { get; set; }
        public bool? IsInsideCheck { get; set; } // ตรวจภายใน
        public string InsideDetail { get; set; }
        public bool? IsPapSmearCheck { get; set; } // มะเร็งปากมดลูก
    }
}