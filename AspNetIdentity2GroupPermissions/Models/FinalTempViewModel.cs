using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace IdentitySample.Models
{
    public class FinalTempViewModel
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [StringLength(150)]
        public string Symbole { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        [StringLength(250)]
        public string Reg { get; set; }

        public string doctype { get; set; }
        public string comm { get; set; }
        public string temptype { get; set; }
        public string cat { get; set; }



      
        [UIHint("GridForeignKey")]
        [DisplayName("Category")]
        public int Cat_ID { get; set; }
    //    public virtual category category { get; set; }

        [UIHint("GridForeignKey")]
        [DisplayName("Document Type")]
        public int Doctype_ID { get; set; }
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