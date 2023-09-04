using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Dtos.Users
{
    public class GetUserDto
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Hn { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string IDCard { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public int? Gender { get; set; }
        public string Agency { get; set; }
        public int? WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        public int? JobTypeID { get; set; }
        public string JobTypeName { get; set; }
        public string PhoneNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        // นับจำนวนโรคจาก Question
        public int Count { get; set; }
        public int? TreatmentID { get; set; }
        public string TreatmentName { get; set; }
        public string UserName { get; set; }
        public string Question40 { get; set; }
        public string Question2q { get; set; }
        public int Question9q { get; set; }
        public string ImageUrl { get; set; }
        public int QuestionSt5 { get; set; }
        public int Question8q { get; set; }
        public int QuestionGHQ28Group1 { get; set; }
        public int QuestionGHQ28Group2 { get; set; }
        public int QuestionGHQ28Group3 { get; set; }
        public int QuestionGHQ28Group4 { get; set; }
    }

    public class FilterUserDto
    {
        public string search { get; set; }
    }
}
