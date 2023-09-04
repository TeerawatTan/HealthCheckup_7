using Microsoft.AspNetCore.Http;
using System;

namespace HelpCheck_API.Dtos.Users
{
    public class EditUserDto
    {
        internal string UserID { get; set; }
        internal string AccessToken { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public string Agency { get; set; }
        public int? WorkPlaceID { get; set; }
        public int? JobTypeID { get; set; }
        public string PhoneNo { get; set; }
        public string Hn { get; set; }
        public int? TreatmentID { get; set; }
        public string ImageUrl { get; set; }
    }
}
