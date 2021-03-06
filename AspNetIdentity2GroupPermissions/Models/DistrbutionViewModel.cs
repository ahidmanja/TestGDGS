﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class DistrbutionViewModel
    {

        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Required]
        public string DName { get; set; }

        [UIHint("GridForeignKey")]
        [DisplayName("Language")]
        public int Lang_ID { get; set; }
    }
}