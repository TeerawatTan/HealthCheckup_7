using System;
using System.Collections.Generic;

namespace HelpCheck_API.Dtos.Dentists
{
    public class AddDentistCheckDto
    {
        internal string AccessToken { get; set; }
        public int MemberID { get; set; }
        public int DoctorID { get; set; }
        public int? Level { get; set; }
        public string Detail { get; set; }
        public List<int>? OralID { get; set; }
    }
}