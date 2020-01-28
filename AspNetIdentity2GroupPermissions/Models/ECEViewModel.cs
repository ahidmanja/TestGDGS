using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class ECEViewModel
    {
        [Required]
        public int Cat { get; set; }

        [Required]
        public int Doctype_ID { get; set; }

        [Required]
        public string SCat { get; set; }

        [DisplayName("Original Language")]
        [Required]
        public int lang_ID { get; set; }


        [DisplayName("Distribution")]
        [Required]
        public string dist { get; set; }

        [DisplayName("Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }


        [DisplayName("Report Number")]
        public string Prep { get; set; }

        [DisplayName("Year")]
        public string CaseYear { get; set; }

        
        [DisplayName("Addendum")]
        public string Add { get; set; }

        [DisplayName("Corrigendum")]
        public string Cor { get; set; }

        
        public bool cAdd { get; set; }        
        public bool cCor { get; set; }

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

        [DisplayName("QR Code")]
        public bool qrcode { get; set; }

        [DisplayName("CPR")]
        public string cprnum { get; set; }
        public bool cpr { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Sdate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Edate { get; set; }

        [DisplayName("Place")]
        public string splace { get; set; }

        [DisplayName("Location")]
        public string loca { get; set; }

        public string tsym { get; set; }
    }
}