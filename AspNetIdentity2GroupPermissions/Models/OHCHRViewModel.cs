using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class OHCHRViewModel
    {
        [Required]
        public int Cat { get; set; }

        [DisplayName("Original Language")]
        [Required]
        public int lang_ID { get; set; }

        [DisplayName("Country")]
        [Required]
        public int count_ID { get; set; }

        [DisplayName("Distribution")]
        [Required]
        public string dist { get; set; }

        [DisplayName("Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd MMMM yyyy}")]
        public DateTime date { get; set; }


        [DisplayName("Report Number")]
        public string Prep { get; set; }

        [DisplayName("Case Number")]
        public string CaseNum { get; set; }
        [DisplayName("Case Year")]
        public string CaseYear { get; set; }

        [DisplayName("Author")]
        public string Author { get; set; }

        [DisplayName("Addendum")]
        public string Add { get; set; }


        [DisplayName("Verisions Required")]
        public List<string> version { get; set; }

        [DisplayName("Verisions Required")]
        public List<string> version1 { get; set; }

     

        [DisplayName("Agenda Item")]
        public string AgendaItem { get; set; }
        [DisplayName("Agenda Number")]
        public string AgendaNum { get; set; }

        [DisplayName("Session Number")]
        public string SNum { get; set; }

        [DisplayName("Session Title")]
        public string STitle { get; set; }

        [DisplayName("QR Code")]
        public bool qrcode { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd MMMM yyyy}")]
        public DateTime? Sdate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{dd MMMM yyyy}")]
        public DateTime? Edate { get; set; }

        public string tsym { get; set; }
    }
}