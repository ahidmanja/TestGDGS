﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class LangViewModel
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Required]
        public string LName { get; set; }

 
    }
}