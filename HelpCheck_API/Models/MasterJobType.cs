﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Models
{
    public class MasterJobType
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
