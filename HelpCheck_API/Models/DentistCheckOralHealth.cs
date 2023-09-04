using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Models
{
    public class DentistCheckOralHealth
    {
        [Key]
        public int DentistCheckID { get; set; }
        
        [Key]
        public int OralID { get; set; }
    }
}
