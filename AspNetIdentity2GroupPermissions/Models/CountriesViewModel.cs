using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace IdentitySample.Models
{
    public class CountriesViewModel
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [Required]
        public string Article { get; set; }
        [Required]
        public string Article1 { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SName { get; set; }
        [Required]
        public string ISO { get; set; }

        [UIHint("GridForeignKey")]
        [DisplayName("Language")]
        public int Lang_ID { get; set; }
    }
}