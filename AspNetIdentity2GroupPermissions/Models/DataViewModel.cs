using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace IdentitySample.Models
{
    public class DataViewModel
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        public string Tlang { get; set; }
        public string Olang { get; set; }
        public string Sdate { get; set; }
        public string Anum { get; set; }
        public string Atitle { get; set; }
        public string Count { get; set; }
        public string Prep { get; set; }
        public string Stitle { get; set; }
        public string Gdoc { get; set; }
        public string Bar { get; set; }
        public string Symh { get; set; }
        public string Dist { get; set; }
        public string Date { get; set; }
        public string Ldate { get; set; }
        public string Dname { get; set; }
        public string Loca { get; set; }
        public string Snum { get; set; }
        public string Mnum { get; set; }
        public string FName { get; set; }
        public string Org { get; set; }
        public string Entity { get; set; }
        public string DocType { get; set; }
        public string Category { get; set; }
        public string Lname1 { get; set; }
        public string Lname2 { get; set; }
        public string Subcat { get; set; }







    }
}