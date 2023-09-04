using System;
using System.Collections.Generic;

namespace HelpCheck_API.Dtos.Dentists
{
    public class GetDentistCheckDto
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int DentistID { get; set; }
        public int? Level { get; set; }
        public string Detail { get; set; }
        public DateTime? CreateDate { get; set; }
        public List<GetDropdownCodeAndNameDto> OralHealths { get; set; }
    }
}