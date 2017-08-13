using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class CatViewModel
    {

        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string doctype { get; set; }
        public string comm { get; set; }
        public string temptype { get; set; }



        [UIHint("GridForeignKey")]
        [DisplayName("Committee")]
        public int DocT_ID { get; set; }
        public virtual doc_type doc_type { get; set; }

        [UIHint("GridForeignKey")]
        [DisplayName("Committee")]
        public int com_ID { get; set; }
        public virtual committee committee { get; set; }

        [UIHint("GridForeignKey")]
        [DisplayName("Template Type")]
        public int temptype_ID { get; set; }
        public virtual template_type template_type { get; set; }
    }
}