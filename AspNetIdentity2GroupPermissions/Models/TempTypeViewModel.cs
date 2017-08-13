using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class TempTypeViewModel
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Required]
        public string TName { get; set; }
    }
}