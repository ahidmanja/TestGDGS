using Humanizer;
using IdentitySample.Models;
using Novacode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class SRTController : Controller
    {
        static Assembly g_assembly;
        static DocX g_document;
        private gdgs1Entities db = new gdgs1Entities();

        // GET: SRT
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindTemplate(SRViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            //Filtering possible Matches according to Symbole structure
            var temp = db.final_temp.Where(x => x.categories_ID == model.Cat).FirstOrDefault();
            string tempname = temp.Name.ToString();
            // var langtemp = db.languages.Where(x => x.ID == model.lang_ID).FirstOrDefault();
            //  string lang = langtemp.Lang_Name.ToString();
            // model.temp = tempname + lang[0].ToString();
            if (model.RC == false)
            {
                model.temp = tempname + "E";
            }
            if (model.RC == true)
            {
                model.temp = tempname + "RE";
            }

            return RedirectToAction("Generate", model);
        }

        public ActionResult Generate(SRViewModel model)
        {

            //get temp name from reg expression plus lang plus symb
            string fname = model.temp.ToString();
            g_assembly = Assembly.GetExecutingAssembly();

            //string source = Server.MapPath(Path.Combine("/" + "IN/CEDAW01SRE.docx"));
            //string Dest = Server.MapPath(Path.Combine("/" + "OUT/CEDAW01SRE.docx"));

            string source = Server.MapPath(Path.Combine("/", "GDGS/IN/" + fname + ".docx"));
            string Dest = Server.MapPath(Path.Combine("/", "GDGS/OUT/" + fname + ".docx"));

            //string tempname = fname.Remove(fname.Length - 1);
            //model.tempname = tempname;
            g_document = DocX.Load(source);
            g_document = CreateDocA(DocX.Load(source), model);

            g_document.SaveAs(Dest);
            if (!String.IsNullOrEmpty(model.author))
            {
                g_document.AddCoreProperty("dc:creator", model.author.ToString());
                g_document.Save();
            }
            return RedirectToAction("download", "Unog", new { name = fname });

        }
        public ActionResult download(string name)
        {
            return File(Url.Content("/GDGS/OUT/" + name + ".docx"), "text/plain", name + ".docx");

            //return File(Url.Content("/OUT/" + name + ".docx"), "text/plain", name + ".docx");
        }
        public static string formatdate(string date, SRViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.languages.FirstOrDefault(p => p.ID == model.lang_ID);
            // string lang = item.Lang_Name.ToString();

            string final = "day month year";
            //if (lang == "French")
            //{
            //    string[] d = date.Split('/');
            //    int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
            //    var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");
            //    string month = frenchCultureInfo.DateTimeFormat.GetMonthName(convert[0]);
            //    if (d[1] == "1")
            //    {
            //        d[1] = d[1] + "\u1D49" + "\u02B3";
            //    }
            //    final = d[1] + " " + month + " " + d[2];
            //}
            //else if (lang == "Spanish")
            //{
            //    string[] d = date.Split('/');
            //    int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
            //    var frenchCultureInfo = CultureInfo.CreateSpecificCulture("es-es");
            //    string month = frenchCultureInfo.DateTimeFormat.GetMonthName(convert[0]);
            //    final = d[1] + " de " + month + " de " + d[2];
            //}
            //else
            //{
            string[] d = date.Split('/');
            int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
            var frenchCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            string month = frenchCultureInfo.DateTimeFormat.GetMonthName(convert[0]);
            final = d[1] + " " + month + " " + d[2];
            //}
            return final;
        }
        public static string formatdate1(SRViewModel model)
        {

            DateTime date = model.ldate;
            string final = "day,d month year,at time";
            string datex = date.ToString();
            string[] d = datex.Split('/');
            // int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
            var frenchCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            string month = frenchCultureInfo.DateTimeFormat.GetMonthName(Convert.ToInt32(d[0]));
            string day = frenchCultureInfo.DateTimeFormat.GetDayName(date.DayOfWeek);
            string[] d2 = d[2].Split(' ');
            string[] d3 = d2[1].Split(':');
            //d2[2]=d
            final = day + ", " + d[1] + " " + month + " " + d2[0] + ", " + model.time.ToString();
            //}
            return final;
        }


        public static string Sessionnum(SRViewModel model)
        {
          
           string final = "";
          
           int num = Convert.ToInt32(model.sNum.ToString());
           if (num <= 99)
              {
                final = num.ToOrdinalWords();
                final = char.ToUpper(final[0]) + final.Substring(1);
            }
           else if (num>=100)
              {
                string TH = "th";

                final = model.sNum.ToString();

                num %= 100;
                if ((num >= 11) && (num <= 13))
                {
                    return final + TH;
                }

                switch (num % 10)
                {
                    case 1: return final + "st";
                    case 2: return final + "nd";
                    case 3: return final + "rd";
                    default: return final + TH;
                }
            }
           

            
            return final;
        }

        public static string getlanguageQR(int id)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.languages.Where(x => x.ID == id).FirstOrDefault();
            //string Cname = count.Lang_Name.ToString();
            //Cname = Cname.Substring(0, 1);
            string Cname = "E";
            return Cname;
        }

        public static string getSym(int CAt)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var tempx = db1.final_temp.Where(x => x.categories_ID == CAt).FirstOrDefault();
            string sym = tempx.Symbole.ToString();
            return sym;
        }
        public static string distrb(int CAt)
        {
            gdgs1Entities db1 = new gdgs1Entities();
            string dist = "";
            var tempx = db1.categories.Where(x => x.ID == CAt).FirstOrDefault();

            string cat = tempx.Name.ToString();
            if (cat == "Public")
            {
                dist = "General";
            }
            if (cat == "Closed")
            {
                dist = "Restricted*";
            }
            return dist;
        }
        //public static string getVerisons(SRViewModel model)
        //{
        //    string[] s = model.ver.Split(',');
        //    // s = SortVirsion(s);

        //    s = s.OrderBy(q => q).ToArray();
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        s[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s[i]);

        //    }


        //    string Fvirs = "";


        //                if (s.Length == 1)
        //                {
        //                    Fvirs = s[0] + " only";
        //                }
        //                if (s.Length == 2)
        //                {
        //                    Fvirs = s[0] + " and " + s[1] + " only";
        //                }
        //                if (s.Length == 3)
        //                {
        //                    Fvirs = s[0] + ", " + s[1] + " and " + s[2] + " only";
        //                }
        //                if (s.Length == 4)
        //                {
        //                    Fvirs = s[0] + ", " + s[1] + ", " + s[2] + " and " + s[3] + " only";
        //                }
        //                if (s.Length == 5)
        //                {
        //                    Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + " and " + s[4] + " only";
        //                }

        //                if (s.Length == 6)
        //                {
        //                    Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + ", " + s[4] + " and " + s[5] + " only";
        //                }

        //    //Upeer case first character 
        //    //Fvirs = char.ToUpper(Fvirs[0]) + Fvirs.Substring(1);
        //    return Fvirs;
        //}

        public static string GetOrdinalSuffix(int number)
        { 
        //{
        //    if (num.ToString().EndsWith("11")) return num + "th";
        //    if (num.ToString().EndsWith("12")) return num + "th";
        //    if (num.ToString().EndsWith("13")) return num + "th";
        //    if (num.ToString().EndsWith("1")) return num + "st";
        //    if (num.ToString().EndsWith("2")) return num + "nd";
        //    if (num.ToString().EndsWith("3")) return num + "rd";
        //    if(num>)
        //    return num + "th";
        string TH = "th";
        string s = number.ToString();

        // Negative and zero have no ordinal representation
            if (number< 1)
            {
                return s;
            }

           number %= 100;
            if ((number >= 11) && (number <= 13))
            {
                return s + TH;
            }

                switch (number % 10)
                {
                    case 1: return s + "st";
                    case 2: return s + "nd";
                    case 3: return s + "rd";
                    default: return s + TH;
                }
        }
        public static string[] info(SRViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.categories.FirstOrDefault(p => p.ID == model.Cat);
            var item1 = db1.doc_type.FirstOrDefault(p => p.ID == item.type_ID);
            var item2 = db1.committees.FirstOrDefault(p => p.ID == item1.committee_ID);
            string[] info = new string[3];
            info[0] = item.Name.ToString();
            info[1] = item1.Name.ToString();
            info[2] = item2.Name.ToString();
            return info;
        }
        public static DocX CreateDocA(DocX template, SRViewModel model)
        {
            //Create Date for Header
            string Fdate = model.date.ToString();
            string[] d = Fdate.Split(' ');
            Fdate = d[0];
            Fdate = formatdate(Fdate, model);
            //Create Distrbution for Header

            string Fdist = distrb(model.Cat);

            string Folang = "English";

            //Session Numer
            string Fsnum = "[NUMBER]";
            if (!String.IsNullOrEmpty(model.sNum))
            {
                Fsnum = Sessionnum(model);
            }

            //sYMBOLE
            string symb = getSym(model.Cat);
            string[] sym = symb.Split(new string[] { "#" }, StringSplitOptions.None);
            string x = sym[1];
            x = model.Prep.ToString() + x;
            string Fsym1 = x;

            string Fmnum = GetOrdinalSuffix(Convert.ToInt32(model.Prep.ToString()));
            //second page meeting date
            string Fsodate = "";
            if (model.time.ToString() == "at 10 a.m.")
            {
                Fsodate = "The meeting was called to order at 10 a.m.";
            }
            if (model.time.ToString() == "at 3 p.m.")
            { Fsodate = "The meeting was called to order at 3 p.m."; }



            //QR code and Bar code

            string lang = getlanguageQR(model.lang_ID);

            string Fsym = getSym(model.Cat);
            //Fsym = Fsym.Remove(Fsym.Length-1);
            // Fsym = Fsym + Fsym1;

            Fsym = Fsym.Replace("#", model.Prep.ToString());

            string url = "http://undocs.org/m2/QRCode.ashx?DS=" + Fsym + "&Size=2&Lang=" + lang;
            using (var client = new WebClient())
            {
                //var content = client.DownloadData("https://api.qrserver.com/v1/create-qr-code/?size=66x66&data=http://undocs.org/fr/A/HRC/70");
                //var content = client.DownloadData("http://undocs.org/m2/QRCode.ashx?DS=A/HRC/70&Size=2&Lang=F");
                var content = client.DownloadData(url);

                using (var str1 = new MemoryStream(content))
                {
                    Image image = template.AddImage(str1);
                    Picture p = image.CreatePicture();
                    Footer f = template.Footers.first;
                    Table t = f.Tables[0];
                    //t.Rows[0].Cells[1].Paragraphs.First().AppendPicture(pR);
                    t.Rows[0].Cells[1].Paragraphs.First().AppendPicture(p);
                }
            }
            string Floc = model.loca.ToString() + ", " + model.locb.ToString();
            //string Fvirs = "";

            //if (model.ver != null)
            //{
            //    Fvirs = getVerisons(model);
            //}

            //create barcode & gdoc
            string Fbar = " ";
            string Fgdoc = " ";
            string gdoc = " ";
            string Fgdocf = " ";
            if (!String.IsNullOrEmpty(model.Gdoc))
            {
                gdoc = model.Gdoc.ToString();
                Fgdoc = gdoc.Insert(2, "-") + "  (E)";
                Fbar = "*" + gdoc + "*";
                Fgdocf = gdoc;
                Fgdocf = gdoc.Insert(2, "-");
                Fgdocf = "GE." + Fgdocf;
            }
            string Fdname = "";
            if (!String.IsNullOrEmpty(model.Cname))
            {
                Fdname = model.Ctitle.ToString() + model.Cname.ToString();
            }
            if (String.IsNullOrEmpty(model.Cname))
            {
                int deleteStart = 0;
                int deleteEnd = 0;

                //Get the array of the paragraphs containing the start and end catches
                for (int i = 0; i < template.Paragraphs.Count; i++)
                {
                    if (template.Paragraphs[i].Text.Contains("Chair:"))
                        deleteStart = i;
                    if (template.Paragraphs[i].Text.Contains("dname"))
                        deleteEnd = i;
                }

                if (deleteStart > 0 && deleteEnd > 0)
                {
                    //delete from the paraIndex as the arrays will shift when a paragraph is deleted
                    int paraIndex = deleteStart;
                    for (int i = deleteStart; i <= deleteEnd; i++)
                    {
                        template.RemoveParagraphAt(paraIndex);
                    }
                }
            }
            if (String.IsNullOrEmpty(model.L1name))
            {
                int deleteStart = 0;
                int deleteEnd = 0;
                int deleteStart1 = 0;
                int deleteEnd1 = 0;
                for (int i = 0; i < template.Paragraphs.Count; i++)
                {
                    if (template.Paragraphs[i].Text.Contains("later") && template.Paragraphs[i].Text.Contains("lname2"))
                        deleteStart1 = i;
                    if (template.Paragraphs[i].Text.Contains("lname2"))
                        deleteEnd1 = i;
                }
                //Get the array of the paragraphs containing the start and end catches
                for (int i = 0; i < template.Paragraphs.Count; i++)
                {
                    if (template.Paragraphs[i].Text.Contains("later") && template.Paragraphs[i].Text.Contains("lname1"))
                        deleteStart = i;
                    if (template.Paragraphs[i].Text.Contains("lname1"))
                        deleteEnd = i;
                }
                if (deleteStart1 > 0 && deleteEnd1 > 0)
                {
                    //delete from the paraIndex as the arrays will shift when a paragraph is deleted
                    int paraIndex = deleteStart1;
                    for (int i = deleteStart1; i <= deleteEnd1; i++)
                    {
                        template.RemoveParagraphAt(paraIndex);
                    }
                }
                if (deleteStart > 0 && deleteEnd > 0)
                {
                    //delete from the paraIndex as the arrays will shift when a paragraph is deleted
                    int paraIndex = deleteStart;
                    for (int i = deleteStart; i <= deleteEnd; i++)
                    {
                        template.RemoveParagraphAt(paraIndex);
                    }
                }
                //Get the array of the paragraphs containing the start and end catches




            }



            if (String.IsNullOrEmpty(model.L2name) && !String.IsNullOrEmpty(model.L1name))
            {
                int deleteStart = 0;
                int deleteEnd = 0;

                for (int i = 0; i < template.Paragraphs.Count; i++)
                {
                    if (template.Paragraphs[i].Text.Contains("later") && template.Paragraphs[i].Text.Contains("lname2"))
                        deleteStart = i;
                    if (template.Paragraphs[i].Text.Contains("lname2"))
                        deleteEnd = i;
                }

                if (deleteStart > 0 && deleteEnd > 0)
                {
                    //delete from the paraIndex as the arrays will shift when a paragraph is deleted
                    int paraIndex = deleteStart;
                    for (int i = deleteStart; i <= deleteEnd; i++)
                    {
                        template.RemoveParagraphAt(paraIndex);
                    }
                }
            }
            string[] info1 = info(model);
            string Fldate = formatdate1(model);
            string xxx = DateTime.Now.ToString();
            template.AddCustomProperty(new CustomProperty("gdoc", Fgdoc));
            template.AddCustomProperty(new CustomProperty("gdocf", Fgdocf));
            template.AddCustomProperty(new CustomProperty("bar", Fbar));

            template.AddCustomProperty(new CustomProperty("osdate", Fsodate));
            template.AddCustomProperty(new CustomProperty("sym1", Fsym1));
            //template. 
            //    template.ReplaceText(Fsym, Fsym, false, RegexOptions.IgnoreCase);

            template.AddCustomProperty(new CustomProperty("symh", Fsym));
            // template.ReplaceText(sym, sym, false, RegexOptions.IgnoreCase);

            template.AddCustomProperty(new CustomProperty("olang", Folang));

            template.AddCustomProperty(new CustomProperty("dist", Fdist));
            //    template.ReplaceText("dist", Fdist, false, RegexOptions.IgnoreCase);

            template.AddCustomProperty(new CustomProperty("date", Fdate));

            template.AddCustomProperty(new CustomProperty("ldate", Fldate));
            //   template.ReplaceText("date", Fdate, false, RegexOptions.IgnoreCase);
            template.AddCustomProperty(new CustomProperty("dname", Fdname));
            //   template.ReplaceText("date", Fdate, false, RegexOptions.IgnoreCase);

            template.AddCustomProperty(new CustomProperty("loca", Floc));
            //   template.ReplaceText("date", Fdate, false, RegexOptions.IgnoreCase);
            //template.AddCustomProperty(new CustomProperty("virs", Fvirs));
            //      template.ReplaceText("virs", "", false, RegexOptions.IgnoreCase);

            template.AddCustomProperty(new CustomProperty("snum", Fsnum));
            //    template.ReplaceText("snum", Fsnum, false, RegexOptions.IgnoreCase);



            template.AddCustomProperty(new CustomProperty("mnum", Fmnum));
            //  template.ReplaceText("prep", Fprep, false, RegexOptions.IgnoreCase);


            template.AddCustomProperty(new CustomProperty("Date-Generated", xxx));


            template.AddCustomProperty(new CustomProperty("Org", "SR"));
            template.AddCustomProperty(new CustomProperty("Entity", info1[0]));
            template.AddCustomProperty(new CustomProperty("doctype", info1[1]));
            template.AddCustomProperty(new CustomProperty("category", info1[2]));

            return template;
        }
      
        public ActionResult WrongTemplate()
        {
            return View();

        }
    }
}

