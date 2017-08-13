using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class UnogViewModel
    {
        [DisplayName("Section")]
        [Required]
        public int lang_ID { get; set; }

        [DisplayName("GDoc Number")]
        [Required]
        public string Gdoc { get; set; }

        [DisplayName("Symbol")]
        [Required]
        public string Sym { get; set; }

        [DisplayName("Distribution")]
        [Required]
        public string dist { get; set; }

        [DisplayName("Verisions Required")]
        public List<string> version { get; set; }

        [DisplayName("Verisions Required")]
        public List<string> version1 { get; set; }
        public string ver { get; set; }

        [DisplayName("Original Language")]
        [Required]
        public int Olang_ID { get; set; }

        [DisplayName("Date")]
        [Required]
        [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{dd MMMM yyyy}")]
        public DateTime date { get; set; }

        //Extra Info

        [DisplayName("Author")]
        public string Author { get; set; }
        [DisplayName("Agenda Item")]
        public string AgendaItem { get; set; }
        [DisplayName("Agenda Number")]
        public string AgendaNum { get; set; }

        [DisplayName("Session Number")]
        public string SNum { get; set; }

        [DisplayName("Session Title")]
        public string STitle { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{dd MMMM yyyy}")]
        public DateTime? Sdate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd MMMM yyyy}")]
        public DateTime? Edate { get; set; }

        //Used for template retrieve

       public int rownum { get; set; }
        public string structure { get; set; } 
        public string regx { get; set; }

        public string tempname { get; set; }
    }
}