using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class UserEditModel
    {
        public int? TitleID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public int? Gender { get; set; }
        public string Agency { get; set; }
        public int? WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        public int? JobTypeID { get; set; }
        public string JobTypeName { get; set; }
        public string PhoneNo { get; set; }
        public string HN { get; set; }
    }
}
