using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace IdentitySample.Models
{
    public class DocTViewModel
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [UIHint("GridForeignKey")]
        [DisplayName("Committee")]
        public int Comm_ID { get; set; }
    }
}