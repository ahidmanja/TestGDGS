using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class JIUViewModel
    {
        [DisplayName("Section")]
        [Required]
        public int lang_ID { get; set; }

        [DisplayName("GDoc Number")]
        public string Gdoc { get; set; }

        [DisplayName("Symbol")]
        [Required]
        public string Sym { get; set; }

        [DisplayName("Original Language")]
        //[Required]
        public int Olang_ID { get; set; }


        //Extra Info

        [DisplayName("Prepared By")]
        public string Pname { get; set; }
        [DisplayName("Title")]
        public string JTitle { get; set; }

  
        //Used for template retrieve

        public int rownum { get; set; }
        public string structure { get; set; }
        public string regx { get; set; }

        public string tempname { get; set; }
    }
}