using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Novacode;
using NVImage = Novacode.Image;
using NVPicture = Novacode.Picture;
using NVTable = Novacode.Table;
using NVFooter = Novacode.Footer;
using System.Reflection;
using System.IO;
using System.Globalization;
using Humanizer;
using System.Net;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging ;
using DocumentFormat.OpenXml.Wordprocessing;

namespace IdentitySample.Controllers
{
    public class ECEController : Controller
    {
        static Assembly g_assembly;
        static DocX g_document;
        private gdgs1Entities db = new gdgs1Entities();

        // GET: ECE
        public ActionResult Index()
        {
            return View();
        }
        public string TypeOfView(int Doctype_ID)
        {
            var temp = db.doc_type.Where(c => c.ID == Doctype_ID).FirstOrDefault();
            string value = temp.Name.ToString();

            if (value == "Working Document")
            {
                return "Working";
            }
            
            else return "General";
        }

        [HttpPost]
        public ActionResult Verify(ECEViewModel model)
        {
            string s = TypeOfView(model.Doctype_ID);
            return View(s, model);
        }
        public static string language(int id)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var language = db1.languages.Where(x => x.ID == id).FirstOrDefault();
            string lname = language.Lang_Name.ToString();
            return lname;
        }
        public static string TempName(ECEViewModel model)
        {
            
            gdgs1Entities db1 = new gdgs1Entities();
            if (model.SCat == "Not Applicable" && model.cpr == false)
            {
                var temp = db1.final_temp.Where(a => a.categories_ID == model.Cat).FirstOrDefault();
                if (temp == null)
                {
                    return "Wrong Template";
                }
                else
                {
                    string lang = language(model.lang_ID);
                    lang = lang.Substring(0, 1);
                    string tempname = temp.Name.ToString() + lang;
                    return tempname;
                }
            }
            else if (model.SCat != "Not Applicable" && model.cpr == false)
            {
                var temp = db1.final_temp.Where(a => a.categories_ID == model.Cat && a.Title == model.SCat).FirstOrDefault();
                if (temp == null)
                {
                    return "Wrong Template";
                }
                else
                {
                    string lang = language(model.lang_ID);
                    lang = lang.Substring(0, 1);
                    string tempname = temp.Name.ToString() + lang;
                    return tempname;
                }
            }
            else if (model.cpr == true && String.IsNullOrEmpty(model.cprnum))
            {

                return "Wrong Template crp";
            }
            else
            {
                string lang = language(model.lang_ID);
                lang = lang.Substring(0, 1);
                string tempname = "ITCCRP"+ lang;
                return tempname;
            }

 
        }
        [HttpPost]
        public ActionResult Generate(ECEViewModel model)
        {
            string fname = TempName(model);
            bool cprvalue = false;
            if (model.cpr == true)
            {
                cprvalue = true;
            }
            if (fname == "Worng Template")
            {
                return RedirectToAction("WrongTemp", "Ohchr");
            }
            if (fname == "Worng Template crp")
            {
                return RedirectToAction("WrongTemp", "Ohchr");
            }

            else
            {
                // Store a global reference to the executing assembly.
                g_assembly = Assembly.GetExecutingAssembly();

                string source = Server.MapPath(Path.Combine("/", "GDGS/IN/" + fname + ".docx"));
                string Dest = Server.MapPath(Path.Combine("/", "GDGS/OUT/" + fname + ".docx"));

               //string source = Server.MapPath(Path.Combine("/", "IN/" + fname + ".docx"));
               // string Dest = Server.MapPath(Path.Combine("/", "OUT/" + fname + ".docx"));

                g_document = DocX.Load(source);
                g_document = CreateDocA(DocX.Load(source), model, source , cprvalue);
                g_document.SaveAs(Dest);

                g_document.AddCoreProperty("dc:title", model.tsym.ToString());
                
                g_document.Save();

                return RedirectToAction("download", "ECE", new { name = fname });
            }
        }

        public ActionResult download(string name)
        {
            return File(Url.Content("/GDGS/OUT/" + name + ".docx"), "text/plain", name + ".docx");
         // return File(Url.Content("/OUT/" + name + ".docx"), "text/plain", name + ".docx");
        }
        public static string getSym(ECEViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();
            if (model.SCat == "Not Applicable")
            {
                var item = db1.final_temp.FirstOrDefault(p => p.categories_ID == model.Cat && p.Title == model.SCat);
                string sym = item.Symbole.ToString();
                return sym;
            }
            else
            {
                var item = db1.final_temp.FirstOrDefault(p => p.categories_ID == model.Cat && p.Title==model.SCat);
                string sym = item.Symbole.ToString();
                return sym;
            }
           
        }
        public static DocX CreateDocA(DocX template, ECEViewModel model,string source, bool cprvalue)
        {
            bool addcor =false;
            string Fadcorr = "";
            bool special = false;
            string Fsnum = "[NUMBER]";
            string sym = getSym(model);
            if (!String.IsNullOrEmpty(model.Prep))
            {
                sym = sym.Replace("#", model.Prep.ToString());
            }
            if (cprvalue == true)
            {
                sym = sym + "/CRP." + model.cprnum.ToString();
            }
            if (!String.IsNullOrEmpty(model.Add) && model.cAdd==true && model.cCor==false)
            {
                sym = sym + "/Add." + model.Add.ToString();
                Fadcorr = "Addendum";
                addcor = true;

            }
            if (!String.IsNullOrEmpty(model.Cor) && model.cCor == true && model.cAdd == false)
            {
                sym = sym + "/Corr." + model.Cor.ToString();
                Fadcorr = "Corrigendum";
                addcor = true;
            }
            if (!String.IsNullOrEmpty(model.Cor) && !String.IsNullOrEmpty(model.Add) && model.cCor == true && model.cAdd == true)
            {
                sym = sym + "/Add." + model.Add.ToString()+"/Corr." + model.Cor.ToString();
                Fadcorr = "Addendum Corrigendum";
                addcor = true;
            }
            if (String.IsNullOrEmpty(model.Cor) && String.IsNullOrEmpty(model.Add))
            {
                
                addcor = false;
            }
            if (String.IsNullOrEmpty(model.CaseYear))
            {
                sym = sym.Replace("&", DateTime.Now.Year.ToString() );
            }
            if (!String.IsNullOrEmpty(model.CaseYear))
            {
                sym = sym.Replace("&", model.CaseYear.ToString());
            }
            model.tsym = sym;
            string[] SymStr = sym.Split('/');
            string[] sym1 = SymStr.Skip(1).ToArray();
            string Fsym = String.Join("/", sym1);

            if (Fsym.Contains("/S/"))
            {
                special = true;
                Fsnum = "special";
            }
            //Create Date for Header
            string Fdate = model.date.ToString();
            string[] d = Fdate.Split(' ');
            Fdate = d[0];
            Fdate = formatdate(Fdate, model);


            //Create Distribution for Header
            string Fdist = getDist(model);

            //Create Virsions for Header
            string Fvirs = "";

            //if (model.version1 != null)
            //{
            //    int deleteStart = 0;
            //    int deleteEnd = 0;

            //    //Get the array of the paragraphs containing the start and end catches
            //    for (int i = 0; i < template.Paragraphs.Count; i++)
            //    {
            //        if (template.Paragraphs[i].Text.Contains("Original"))
            //            deleteStart = i;
            //        if (template.Paragraphs[i].Text.Contains("olang"))
            //            deleteEnd = i;
            //    }

            //    if (deleteStart > 0 && deleteEnd > 0)
            //    {
            //        //delete from the paraIndex as the arrays will shift when a paragraph is deleted
            //        int paraIndex = deleteStart;
            //        for (int i = deleteStart; i <= deleteEnd; i++)
            //        {
            //            template.RemoveParagraphAt(paraIndex);
            //        }
            //    }
            //}

            Fvirs = getVerisons(model);

            string Folang = "";
            Folang = Olanguage(model.lang_ID);

            string Fatitle = "[Title]";
            if (!String.IsNullOrEmpty(model.AgendaItem))
            {
                Fatitle = model.AgendaItem.ToString();
            }
            string Fsdate = "[Start-End Dates ]";
            if (model.Sdate != null & model.Edate != null)
            {
                string sdate = model.Sdate.ToString();
                string[] sd = sdate.Split(' ');
                sdate = sd[0];
                sdate = formatdate(sdate, model);

                string edate = model.Edate.ToString();
                string[] ed = edate.Split(' ');
                edate = ed[0];
                edate = formatdate(edate, model);


                Fsdate = SessionDate(sdate, edate, model);
            }
            string Fldate = "[Start Date]";
            if (model.Sdate != null & model.Edate != null)
            {
                string ldate = model.Sdate.ToString();
                string[] ld = ldate.Split(' ');
                ldate = ld[0];
               string  dldate = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(ldate).DayOfWeek);
                ldate = formatdate(ldate, model);
                //Regex rx = new Regex(@"^\d+\p{Zs}", RegexOptions.Compiled);
                //ldate = rx.Replace(ldate, dldate+" ");
                ldate = dldate + " " + ldate;
        Fldate = ldate;
            }

            string Fanum = "[NUMBER]";
            if (!String.IsNullOrEmpty(model.AgendaNum))
            {
                Fanum = model.AgendaNum.ToString();
            }
            
            if (special == false)
            {
               
                if (!String.IsNullOrEmpty(model.SNum))
                {
                    Fsnum = Sessionnum(model);
                }
            }
           
            

            if (model.qrcode == true)
            {
                string lang = language(model.lang_ID);
                lang = lang.Substring(0, 1);


                string url = "http://undocs.org/m2/QRCode.ashx?DS=" + Fsym + "&Size=2&Lang=" + lang;
                using (var client = new WebClient())
                {
                    //var content = client.DownloadData("https://api.qrserver.com/v1/create-qr-code/?size=66x66&data=http://undocs.org/fr/A/HRC/70");
                    //var content = client.DownloadData("http://undocs.org/m2/QRCode.ashx?DS=A/HRC/70&Size=2&Lang=F");
                    var content = client.DownloadData(url);

                    using (var str = new MemoryStream(content))
                    {
                       
                        if (lang == "A")
                        {
                            Image image = template.AddImage(str);
                            NVPicture p = image.CreatePicture();
                            NVFooter f = template.Footers.first;
                            NVTable t = f.Tables[0];
                            //t.Rows[0].Cells[1].Paragraphs.First().AppendPicture(pR);
                            t.Rows[0].Cells[0].Paragraphs.First().AppendPicture(p);
                        }
                        else
                        {

                            NVImage image = template.AddImage(str);
                            NVPicture p = image.CreatePicture();
                            NVFooter f = template.Footers.first;
                            NVTable t = f.Tables[0];
                            //t.Rows[0].Cells[1].Paragraphs.First().AppendPicture(pR);
                            t.Rows[0].Cells[1].Paragraphs.First().AppendPicture(p);
                        }
                    }
                }
            }
            string Fcategory = "";
            Fcategory = getCat(model.Cat);
            string FSubcategory = "";
            if (model.SCat != "Not Applicable" || model.SCat != "Special Session")
            {
                FSubcategory = model.SCat.ToString();
            }
            if (model.SCat == "Not Applicable" || model.SCat == "Special Session")
            {
                int deleteStart = 0;
                int deleteEnd = 0;

                //Get the array of the paragraphs containing the start and end catches
                for (int i = 0; i < template.Paragraphs.Count; i++)
                {
                    if (template.Paragraphs[i].Text.Contains("subcategory"))
                        deleteStart = i;
                    if (template.Paragraphs[i].Text.Contains("subcategory"))
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
            if (addcor==false)
            {
                int deleteStart = 0;
                int deleteEnd = 0;

                //Get the array of the paragraphs containing the start and end catches
                for (int i = 0; i < template.Paragraphs.Count; i++)
                {
                    if (template.Paragraphs[i].Text.Contains("adcorr"))
                        deleteStart = i;
                    if (template.Paragraphs[i].Text.Contains("adcorr"))
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
            string Fsplace = "[Session City]";
            if (!String.IsNullOrEmpty(model.splace))
            {
                Fsplace = model.splace.ToString();
            }
            string Floca = "[Session Location]";
            if (!String.IsNullOrEmpty(model.loca))
            {

                Floca = model.loca.ToString()+", "+ Fsplace;
            }

            DateTime x = DateTime.Now;
            string[] info1 = info(model);

            template.AddCustomProperty(new CustomProperty("sym1", Fsym));
            template.AddCustomProperty(new CustomProperty("symh", sym));
            template.AddCustomProperty(new CustomProperty("dist", Fdist));
            template.AddCustomProperty(new CustomProperty("date", Fdate));
            template.AddCustomProperty(new CustomProperty("sdate", Fsdate));
            template.AddCustomProperty(new CustomProperty("virs", Fvirs));
            template.AddCustomProperty(new CustomProperty("snum", Fsnum));
            template.AddCustomProperty(new CustomProperty("anum", Fanum));
            template.AddCustomProperty(new CustomProperty("adcorr", Fadcorr));
            template.AddCustomProperty(new CustomProperty("gdoc", ""));
            template.AddCustomProperty(new CustomProperty("gdocf", ""));
            template.AddCustomProperty(new CustomProperty("tlang", ""));
            template.AddCustomProperty(new CustomProperty("atitle", Fatitle));
            template.AddCustomProperty(new CustomProperty("ldate", Fldate));
            template.AddCustomProperty(new CustomProperty("loca", Floca));
            template.AddCustomProperty(new CustomProperty("categ", Fcategory));
            template.AddCustomProperty(new CustomProperty("subcategory",FSubcategory));
            template.AddCustomProperty(new CustomProperty("splace", Fsplace));
            template.AddCustomProperty(new CustomProperty("olang", Folang));
            template.AddCustomProperty(new CustomProperty("Date-Generated", x));
            template.AddCustomProperty(new CustomProperty("Org", "ECE"));
            template.AddCustomProperty(new CustomProperty("Entity",info1[0] ));
            template.AddCustomProperty(new CustomProperty("doctype", info1[1]));
            template.AddCustomProperty(new CustomProperty("category", info1[2]));
            template.AddCustomProperty(new CustomProperty("bar", ""));

            for (int i = 0; i < template.Paragraphs.Count; i++)
            {
                if (template.Paragraphs[i].Text.Contains(Fsnum))
                {
                    string Fsnum1= char.ToUpper(Fsnum[0]) + Fsnum.Substring(1);
                    template.Paragraphs[i].ReplaceText(Fsnum, Fsnum1);
                    break;
                }
                    
            }
            if (addcor==true && cprvalue==false)
            {


                int deleteStart = 0;
                int deleteEnd = 0;

                //Get the array of the paragraphs containing the start and end catches
                for (int i = 0; i < template.Paragraphs.Count; i++)
                {
                    if (template.Paragraphs[i].Text.Contains("*"))
                        deleteStart = i;
                  
                    if (template.Paragraphs[i].Text.Contains(Fadcorr))
                        deleteEnd = i;
                }

                if (deleteStart > 0 && deleteEnd > 0)
                {
                    //delete from the paraIndex as the arrays will shift when a paragraph is deleted
                    int paraIndex = deleteEnd - deleteStart;
                    paraIndex = deleteStart + paraIndex - 1;

                    template.RemoveParagraphAt(paraIndex);

                }
                //List <string> replace = template.FindUniqueByPattern("*", RegexOptions.None);
                //foreach (string s in replace)
                //{
                //    if (!s.Equals("")) template.ReplaceText((s, "");
                //}

                template.ReplaceText("*", "");

               List<string> repl =template.FindUniqueByPattern(",\\s", RegexOptions.None);
                foreach (string rep in repl)
                {
                    if (!String.IsNullOrEmpty(rep))
                    {
                        template.ReplaceText(rep, "");
                    }
                }





                //bool flag = false;
                //List<List<string>> list1 = new List<List<string>>();
                //List<string> list2 = new List<string>();
                //foreach (Novacode.Paragraph item in template.Paragraphs)
                //{
                //    //use this if you need whole text of a paragraph
                //    string paraText = item.Text;
                //    var result = paraText.Split(' ');
                //    int count = 0;
                //    list2 = new List<string>();
                //    //use this if you need word by word
                //    foreach (var data in result)
                //    {
                //        string word = data.ToString();

                //        if (word.Contains(Fsnum)) flag = true;
                //        if (word.Contains("session"))
                //        {
                //            flag = false;
                //            // list2.Add(word);
                //        }
                //        if (flag)
                //            list2.Add(word);
                //        count++;
                //    }
                //    //    list2.RemoveAt(0);

                //    list1.Add(list2);

                //}
                ////   list1[0].Replace(list1[0], "*");
                //for (int i = 0; i < list1.Count(); i++)
                //{

                //    string temp = "";
                //    for (int y = 0; y < list1[i].Count(); y++)
                //    {
                //        if (y == 0)
                //        {
                //            temp = list1[i][y];
                //            continue;
                //        }
                //        temp += " " + list1[i][y];
                //        //   temp = temp.Remove(temp.IndexOf(',')-1, temp.Length);
                //    }
                //    int ind = temp.IndexOf("*");

                //    // temp = temp.Substring(ind, temp.Length - ind);
                //    if (!temp.Equals("")) template.ReplaceText("Done", "");
                //}


            }


            // Return the template now that it has been modified to hold all of our custom data.
            return template;

        }
        public static string getCat(int id)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.categories.FirstOrDefault(p => p.ID == id);
            string final = item.Name.ToString();
            return final;
        }
        public static string Sessionnum(ECEViewModel model)
        {
            string lang = language(model.lang_ID);
            string final = "";
            switch (lang)
            {
                case "English":
                    {

                        int num = Convert.ToInt32(model.SNum.ToString());
                        if (num <= 99)
                        {
                            final = num.ToOrdinalWords();
                        }
                        else
                        {
                            final = model.SNum.ToString() + "th";
                        }
                        break;
                    }
                case "French":
                    {
                        var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");
                        int num = Convert.ToInt32(model.SNum.ToString());
                        if (num <= 99)
                        {
                            final = num.ToOrdinalWords(frenchCultureInfo);
                        }
                        else
                        {
                            final = model.SNum.ToString() + "\u1D49";
                        }
                        break;
                    }
                case "Spanish":
                    {
                        var spanishCultureInfo = CultureInfo.CreateSpecificCulture("es-es");
                        int num = Convert.ToInt32(model.SNum.ToString());
                        if (num <= 10)
                        {
                            if (num == 1 || num == 3)
                            {
                                final = Toordinalspanish(num.ToString());
                                final = final.Remove(final.Length - 1);
                            }
                            // final = num.ToOrdinalWords(spanishCultureInfo);
                            else
                            {
                                final = Toordinalspanish(num.ToString());
                            }

                        }
                        else
                        {
                            final = model.SNum.ToString();
                            string chr = final.Last().ToString();
                            if (chr == "1" || chr == "3")
                            {
                                final = final + "\u1D49" + "\u02B3";
                            }
                            else
                            {
                                final = final + "\u1D52";
                            }

                        }
                        break;
                    }
                default:
                    {
                        final = "Not Supported";
                        break;
                    }
            }

            //final = char.ToUpper(final[0]) + final.Substring(1);
            return final;
        }
        public static string Toordinalspanish(string num)
        {
            string final = "";
            string[] Unidad = { "", "primero", "segundo", "tercero",
            "cuarto", "quinto", "sexto", "séptimo", "octavo",
            "noveno" };

            string[] Decena = { "", "décimo", "vigésimo", "trigésimo",
            "cuadragésimo", "quincuagésimo", "sexagésimo",
            "septuagésimo", "octogésimo", "nonagésimo" };
            string[] Centena = {"", "centésimo", "ducentésimo",
            "tricentésimo", " cuadringentésimo", " quingentésimo",
            " sexcentésimo", " septingentésimo", " octingentésimo",
            " noningentésimo"};

            int N = Convert.ToInt32(num);
            int u = N % 10;
            int d = (N / 10) % 10;
            int c = N / 100;
            if (N >= 100)
            {
                final = Centena[c] + " " + Decena[d] + " " + Unidad[u];
            }
            else
            {
                if (N >= 10 && N <= 20)
                {
                    if (N == 11)
                    {
                        final = "undécimo";
                    }
                    if (N == 12)
                    {
                        final = "duodécimo";
                    }
                    else if (N >= 13 && N <= 19)
                    {
                        final = "decimo" + Unidad[u];
                    }

                }
                if (N > 20)
                {
                    final = Decena[d] + " " + Unidad[u];
                }
                else if (N < 10)
                {
                    final = Unidad[N];
                }
            }
            return final;
        }
        public static string SessionDate(string sdate, string edate, ECEViewModel model)
        {
            string final = "";
            string[] s = sdate.Split(' ');
            string[] e = edate.Split(' ');
            string lang = language(model.lang_ID);

            switch (lang)
            {
                case "Spanish":
                    {
                        if (s[2] == e[2])
                        {
                            final = s[0] + " a " + e[0] + " de " + e[2] + " de " + e[4];
                        }
                        if (s[2] != e[2])
                        {

                            final = s[0] + " de " + s[2] + " a " + e[0] + " de " + e[2] + " " + e[4];
                        }
                        //return final;
                        break;

                    }
                default:
                    {
                        if (s[1] == e[1])
                        {
                            final = s[0] + "-" + e[0] + " " + e[1] + " " + e[2];
                        }
                        if (s[1] != e[1])
                        {

                            final = s[0] + " " + s[1] + "-" + e[0] + " " + e[1] + " " + e[2];
                        }
                        //return final;
                        break;
                    }
            }
            return final;
        }
        public static string Olanguage(int id)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var language = db1.languages.Where(x => x.ID == id).FirstOrDefault();
            string lname = language.Lang_Name.ToString();
            switch (lname)
            {
                case "French":
                    {
                        lname = "français";
                        break;
                    }
                case "Spanish":
                    {
                        lname = "español";
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
            return lname;
        }
        public static string getVerisons(ECEViewModel model)
        {
            string[] s;
            if (model.version1 == null)
            {
                string ss = Olanguage(model.lang_ID);
                s = ss.Split(',');
            }
            else
            {
                s = model.version1.ToArray();
                string a = Olanguage(model.lang_ID);
                Array.Resize(ref s, s.Length + 1);
                s[s.Length - 1] = a;
            }


            // s = SortVirsion(s);

            s = s.OrderBy(q => q).ToArray();





            for (int i = 0; i < 1; i++)
            {
                s[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s[i]);

            }
            string lang = language(model.lang_ID);
            string Fvirs = "";

            switch (lang)
            {
                case "French":
                    {

                        if (s.Length == 1)
                        {
                            Fvirs = s[0] + " seulement";
                        }
                        if (s.Length == 2)
                        {
                            Fvirs = s[0] + " et " + s[1] + " seulement";
                        }
                        if (s.Length == 3)
                        {
                            Fvirs = s[0] + ", " + s[1] + " et " + s[2] + " seulement";
                        }
                        if (s.Length == 4)
                        {
                            Fvirs = s[0] + ", " + s[1] + ", " + s[2] + " et " + s[3] + " seulement";
                        }
                        if (s.Length == 5)
                        {
                            Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + " et " + s[4] + " seulement";
                        }

                        if (s.Length == 6)
                        {
                            Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + ", " + s[4] + " et " + s[5] + " seulement";
                        }
                        break;
                    }
                case "Spanish":
                    {

                        if (s.Length == 1)
                        {
                            Fvirs = s[0] + " únicamente";
                        }
                        if (s.Length == 2)
                        {
                            if (s[1] == "inglés")
                            {
                                Fvirs = s[0] + " e " + s[1] + " únicamente";
                            }
                            else
                            {
                                Fvirs = s[0] + " y " + s[1] + " únicamente";
                            }

                        }
                        if (s.Length == 3)
                        {
                            if (s[2] == "inglés")
                            {
                                Fvirs = s[0] + ", " + s[1] + " e " + s[2] + " únicamente";
                            }
                            else
                            {
                                Fvirs = s[0] + ", " + s[1] + " y " + s[2] + " únicamente";
                            }
                        }
                        if (s.Length == 4)
                        {
                            if (s[3] == "inglés")
                            {
                                Fvirs = s[0] + ", " + s[1] + ", " + s[2] + " y " + s[3] + " únicamente";
                            }
                            else
                            {
                                Fvirs = s[0] + ", " + s[1] + ", " + s[2] + " e " + s[3] + " únicamente";
                            }
                        }
                        if (s.Length == 5)
                        {
                            if (s[4] == "inglés")
                            {
                                Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + " e " + s[4] + " únicamente";
                            }
                            else
                            {
                                Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + " y " + s[4] + " únicamente";
                            }
                        }

                        if (s.Length == 6)
                        {
                            if (s[5] == "inglés")
                            {
                                Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + ", " + s[4] + " e " + s[5] + " únicamente";
                            }
                            else
                            {
                                Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + ", " + s[4] + " y " + s[5] + " únicamente";
                            }
                        }
                        break;
                    }

                default:
                    {
                        if (s.Length == 1)
                        {
                            Fvirs = s[0] + " only";
                        }
                        if (s.Length == 2)
                        {
                            Fvirs = s[0] + " and " + s[1] + " only";
                        }
                        if (s.Length == 3)
                        {
                            Fvirs = s[0] + ", " + s[1] + " and " + s[2] + " only";
                        }
                        if (s.Length == 4)
                        {
                            Fvirs = s[0] + ", " + s[1] + ", " + s[2] + " and " + s[3] + " only";
                        }
                        if (s.Length == 5)
                        {
                            Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + " and " + s[4] + " only";
                        }

                        if (s.Length == 6)
                        {
                            Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + ", " + s[4] + " and " + s[5] + " only";
                        }
                        break;
                    }
            }
            //Upeer case first character 
            Fvirs = char.ToUpper(Fvirs[0]) + Fvirs.Substring(1);
            return Fvirs;


        }
        public static string getDist(ECEViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.distrbutions.FirstOrDefault(p => p.ID.ToString() == model.dist);
            string dist = item.Name.ToString();
            return dist;
        }

        public static string[] info(ECEViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.categories.FirstOrDefault(p => p.ID == model.Cat);
            var item1= db1.doc_type.FirstOrDefault(p => p.ID == item.type_ID);
            var item2= db1.committees.FirstOrDefault(p => p.ID == item1.committee_ID);
            string[] info  = new string[3];
            info[0] = item.Name.ToString();
            info[1] = item1.Name.ToString();
            info[2] = item2.Name.ToString();
            return info;
        }
        public static string formatdate(string date, ECEViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.languages.FirstOrDefault(p => p.ID == model.lang_ID);
            string lang = item.Lang_Name.ToString();

            string final = "day month year";
            if (lang == "French")
            {
                string[] d = date.Split('/');
                int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
                var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");
                string month = frenchCultureInfo.DateTimeFormat.GetMonthName(convert[0]);
                if (d[1] == "1")
                {
                    d[1] = d[1] + "\u1D49" + "\u02B3";
                }
                final = d[1] + " " + month + " " + d[2];
            }
            else if (lang == "Spanish")
            {
                string[] d = date.Split('/');
                int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
                var frenchCultureInfo = CultureInfo.CreateSpecificCulture("es-es");
                string month = frenchCultureInfo.DateTimeFormat.GetMonthName(convert[0]);
                final = d[1] + " de " + month + " de " + d[2];
            }
            else
            {
                string[] d = date.Split('/');
                int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
                var frenchCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                string month = frenchCultureInfo.DateTimeFormat.GetMonthName(convert[0]);
                final = d[1] + " " + month + " " + d[2];
            }
            return final;
        }


        public ActionResult Working()
        {
            return View();
        }



        public ActionResult General()
        {
            return View();
        }
    }
}