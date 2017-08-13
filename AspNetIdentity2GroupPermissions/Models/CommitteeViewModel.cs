using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class CommitteeViewModel
    {

        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [UIHint("GridForeignKey")]
        [DisplayName("Template Type")]
        public int TType_ID { get; set; }
    }
}