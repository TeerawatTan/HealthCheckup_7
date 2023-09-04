using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class DropdownModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class AddDropDownDto
    {
        public string Name { get; set; }
    }
}
