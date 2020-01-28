using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class SRViewModel
    {
        [Required]
        public int Cat { get; set; }

        [DisplayName("Original Language")]
        [Required]
        public int lang_ID { get; set; }

        public string temp { get; set; }

        //[DisplayName("Distribution")]
        //[Required]
        //public string dist { get; set; }

        [DisplayName("Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd MMMM yyyy}")]
        public DateTime date { get; set; }

        [Required]
        [DisplayName("Report Number")]
        public string Prep { get; set; }
        [Required]
        [DisplayName("Time")]
        public string time { get; set; }

       
        [DisplayName("Resumed Coverage")]
        public bool RC { get; set; }

        //[DisplayName("Verisions Required")]
        //public List<string> version { get; set; }

        //public string ver { get; set; }

        //[DisplayName("Verisions Required")]
        //public List<string> version1 { get; set; }

        [Required]
        [DisplayName("Session Number")]
        public string sNum { get; set; }

        [Required]
        [DisplayName("Location")]
        public string loca { get; set; }

        [Required]
        public string locb { get; set; }


        [Required]
        [DisplayName("Title")]
        public string Ctitle { get; set; }
        [Required]
        [DisplayName("Name")]
        public string Cname { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ldate { get; set; }


    }
}