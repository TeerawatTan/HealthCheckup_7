using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        public int? TitleID { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDCard { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Agency { get; set; }
        internal int AgencyID 
        { 
            get 
            {
                try
                {
                    return Int32.Parse(Agency);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        public int? WorkPlaceID { get; set; }
        public int? JobTypeID { get; set; }
        public string PhoneNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string Token { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? RoleID { get; set; }
        public string Hn { get; set; }
        public int? TreatmentID { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
    }
}
