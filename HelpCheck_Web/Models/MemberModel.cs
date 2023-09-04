using System;
using System.Collections.Generic;

namespace HelpCheck_Web.Models
{
    public class MemberModel
    {
        public int memberId { get; set; }
        public string fullName { get; set; }
        public string birthDateStr { get; set; }
        public string agencyName { get; set; }
        public string jobTypeName { get; set; }
        public string workPlaceName { get; set; }
        public string profileImg { get; set; }
    }
}
