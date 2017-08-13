using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Novacode;
using System.Reflection;
using System.IO;
using System.Globalization;
using Humanizer;
using System.Net;


namespace IdentitySample.Controllers
{
    public class OHCHRController : Controller
    {
        static Assembly g_assembly;
        static DocX g_document;
        private gdgs1Entities db = new gdgs1Entities();

        // GET: OHCHR
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Verify(OHCHRViewModel model)
        {
            string s = TypeOfView(model.Cat);
            if (s == "Add")
            {
                return View("Add", model);
            }
            if (s == "Communication")
            {
                return View("Communication", model);
            }
            else
            {
                return View("Report", model);
            }
        }


        [HttpPost]
        public ActionResult Generate(OHCHRViewModel model)
        {
            string fname = TempName(model);
            if (fname == "Worng Template")
            {
                return RedirectToAction("WrongTemp", "Ohchr");
            }

            else
            {
                // Store a global reference to the executing assembly.
                g_assembly = Assembly.GetExecutingAssembly();

              //  string source = Server.MapPath(Path.Combine("/", "GDGS/IN/" + fname + ".docx"));
               // string Dest = Server.MapPath(Path.Combine("/", "GDGS/OUT/" + fname + ".docx"));

                string source = Server.MapPath(Path.Combine("/", "IN/" + fname + ".docx"));
                string Dest = Server.MapPath(Path.Combine("/", "OUT/" + fname + ".docx"));

                g_document = DocX.Load(source);
                g_document = CreateDocA(DocX.Load(source), model);
                
                g_document.SaveAs(Dest);

                g_document.AddCoreProperty("dc:title", model.tsym.ToString());
                g_document.Save();

                return RedirectToAction("download", "OHCHR", new { name = fname });
            }
        }

        public ActionResult download(string name)
        {
       //  return File(Url.Content("/GDGS/OUT/" + name + ".docx"), "text/plain", name + ".docx");
          return File(Url.Content("/OUT/" + name + ".docx"), "text/plain", name + ".docx");
        }
        public static DocX CreateDocA(DocX template, OHCHRViewModel model)
        {
            string iso = getIso(model);
            string sym = getSym(model);

            if (!String.IsNullOrEmpty(model.Prep))
            {

                sym = sym.Replace("#", model.Prep.ToString());
            }
            if (!String.IsNullOrEmpty(model.Add))
            {

                sym = sym.Replace("$", model.Add.ToString());
            }
            if (iso != "")
            {
                sym = sym.Replace("ISO", iso);
            }
            if (!String.IsNullOrEmpty(model.CaseNum))
            {
                sym = sym.Replace("*", model.CaseNum.ToString());
            }
            if (!String.IsNullOrEmpty(model.CaseYear))
            
            {
                sym = sym.Replace("&", model.CaseYear.ToString());
            }
            model.tsym = sym;
            string[] SymStr = sym.Split('/');
            string[] sym1 = SymStr.Skip(1).ToArray();
            string Fsym = String.Join("/", sym1);

            //Create Date for Header
            string Fdate = model.date.ToString();
            string[] d = Fdate.Split(' ');
            Fdate = d[0];
            Fdate = formatdate(Fdate,model);

            //Create Distrbution for Header
            string Fdist = getDist(model);

            //Create Virsions for Header
            string Fvirs = "";

            if (model.version1 != null)
            {
                Fvirs = getVerisons(model);
            }

            string Folang = "";
            Folang = Olanguage(model.lang_ID);

            string FAuthor = "";
            if (!String.IsNullOrEmpty(model.Author))
            {
                FAuthor = model.Author.ToString();
            }

            string Fatitle = "[Title]";
            if (!String.IsNullOrEmpty(model.AgendaItem))
            {
                Fatitle = model.AgendaItem.ToString();
            }
            string Fstitle = "[Title]";
            if (!String.IsNullOrEmpty(model.STitle))
            {
                Fstitle = model.STitle.ToString();
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


                  Fsdate = SessionDate(sdate, edate , model);
            }

            string Fanum = "[NUMBER]";
            if (!String.IsNullOrEmpty(model.AgendaNum))
            {
                Fanum = model.AgendaNum.ToString();
            }

            string Fsnum = "[NUMBER]";
            if (!String.IsNullOrEmpty(model.SNum))
            {
                Fsnum = Sessionnum(model);
            }

            string Fcount = "";
            string fCountW = "";
            string fCountWD = "";
            if (model.count_ID!=null)
            {

                Fcount = getCount(model);
                fCountW = getCountW(model);
                fCountWD = getCountWD(model);

            }
            string Fprep = "";
            string FprepW = "";
            string FprepWC = "";
            if (!String.IsNullOrEmpty(model.Prep))
            {
                Fprep = Reportnumber(model);
                FprepW = ReportnumberW(model);
                FprepWC = ReportnumberWC(model);
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
                        Image image = template.AddImage(str);
                        Picture p = image.CreatePicture();
                        Footer f = template.Footers.first;
                        Table t = f.Tables[0];
                        //t.Rows[0].Cells[1].Paragraphs.First().AppendPicture(pR);
                        t.Rows[0].Cells[1].Paragraphs.First().AppendPicture(p);
                    }
                }
            }
            DateTime x = DateTime.Now;

            template.AddCustomProperty(new CustomProperty("sym1", Fsym));
            template.AddCustomProperty(new CustomProperty("symh", sym));
            template.AddCustomProperty(new CustomProperty("dist", Fdist));
            template.AddCustomProperty(new CustomProperty("date", Fdate));
           template.AddCustomProperty(new CustomProperty("sdate", Fsdate));
            template.AddCustomProperty(new CustomProperty("virs", Fvirs));
            template.AddCustomProperty(new CustomProperty("snum", Fsnum));
            template.AddCustomProperty(new CustomProperty("anum", Fanum));
            template.AddCustomProperty(new CustomProperty("count", Fcount));
            template.AddCustomProperty(new CustomProperty("countw", fCountW));
            template.AddCustomProperty(new CustomProperty("countwd", fCountWD));
            template.AddCustomProperty(new CustomProperty("gdoc", ""));
            template.AddCustomProperty(new CustomProperty("gdocf", ""));
            template.AddCustomProperty(new CustomProperty("tlang", ""));
            template.AddCustomProperty(new CustomProperty("atitle", Fatitle));
            template.AddCustomProperty(new CustomProperty("stitle", Fstitle));
            template.AddCustomProperty(new CustomProperty("prep", Fprep));
            template.AddCustomProperty(new CustomProperty("prepw", FprepW));
            template.AddCustomProperty(new CustomProperty("prepwc", FprepWC));
            template.AddCustomProperty(new CustomProperty("olang", Folang));
           template.AddCustomProperty(new CustomProperty("Date-Generated", x));
            template.AddCustomProperty(new CustomProperty("Author", FAuthor));
            template.AddCustomProperty(new CustomProperty("bar", ""));

            // Return the template now that it has been modified to hold all of our custom data.
            return template;


        }
        public static string ReportnumberWC(OHCHRViewModel model)
        {
            string lang = language(model.lang_ID);
            string final = "";
            //  string country = getCount(model);
            //string Country1 = getCountwithSrticle(model);
            //bool cat = false;
            //cat = categoryname(model);
            //if (cat == true)
            //{
            //switch (lang)
            //{
            //    case "English":
            //        {
            //            string cc = model.Prep.ToString();
            //            string[] com = cc.Split('-');

            //            //case single number
            //            if (com.Length == 1)
            //            {
            //                var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            //                int x = Convert.ToInt32(model.Prep.ToString());
            //                final = x.ToOrdinalWords(engCultureInfo);
            //                if (final == "first")
            //                {
            //                    final = "initial report " + Country1;
            //                }
            //                else { final = final + " periodic report " + Country1; }
            //            }

            //            //case combined number
            //            if (com.Length > 1)
            //            {
            //                var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            //                int x = Convert.ToInt32(com[0]);
            //                int y = Convert.ToInt32(com[1]);
            //                string Fprep1 = x.ToOrdinalWords(engCultureInfo);
            //                string Fprep2 = y.ToOrdinalWords(engCultureInfo);
            //                //case 4 and 5 or 5 and 6
            //                if (x == y - 1)
            //                {
            //                    if (Fprep1 == "first")
            //                    {
            //                        Fprep1 = "initial ";
            //                        final = Fprep1 + "and " + Fprep2 + " combined reports " + Country1;
            //                    }
            //                    else { final = Fprep1 + " and " + Fprep2 + " combined reports " + Country1; }
            //                }
            //                //case 1 - 5 or 3-10
            //                if (x != y - 1)
            //                {
            //                    if (Fprep1 == "first")
            //                    {
            //                        Fprep1 = "initial ";
            //                        final = Fprep1 + " to " + Fprep2 + " combined reports " + Country1;
            //                    }
            //                    else { final = Fprep1 + " to " + Fprep2 + " combined reports " + Country1; }
            //                }

            //            }
            //            break;
            //        }
            //    case "French":
            //        {
            //            string cc = model.Prep.ToString();
            //            string[] com = cc.Split('-');
            //            var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

            //            //case single number
            //            if (com.Length == 1)
            //            {
            //                int x = Convert.ToInt32(model.Prep.ToString());
            //                final = x.ToOrdinalWords(frenchCultureInfo);
            //                if (final == "premier")
            //                {
            //                    final = "rapport initial " + Country1;
            //                }
            //                else { final = final + " rapport périodique " + Country1; }
            //            }
            //            //case combined number
            //            if (com.Length > 1)
            //            {
            //                int x = Convert.ToInt32(com[0]);
            //                int y = Convert.ToInt32(com[1]);
            //                string Fprep1 = x.ToOrdinalWords(frenchCultureInfo);
            //                string Fprep2 = y.ToOrdinalWords(frenchCultureInfo);
            //                //case 4 and 5 or 5 and 6


            //                if (Fprep1 == "premier")
            //                {
            //                    Fprep1 = "rapport initial";
            //                    final = "rapport " + Country1 + " valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
            //                }
            //                else { final = "rapport " + Country1 + " valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }


            //            }
            //            break;
            //        }
            //    case "Spanish":
            //        {
            //            final = SpanishNum(model);

            //            break;
            //        }
            //}
            //}
            //if(cat==false)
            //{
            switch (lang)
            {
                case "English":
                    {
                        string cc = model.Prep.ToString();
                        string[] com = cc.Split('-');

                        //case single number
                        if (com.Length == 1)
                        {
                            // var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                            int x = Convert.ToInt32(model.Prep.ToString());
                            final = x.ToOrdinalWords();
                            final = final.Substring(0, 1).ToUpperInvariant() + final.Substring(1);
                            if (final == "First")
                            {
                                final = "Initial report ";
                            }
                            else { final = final + " periodic report "; }
                        }

                        //case combined number
                        if (com.Length > 1)
                        {
                            int x = Convert.ToInt32(com[0]);
                            int y = Convert.ToInt32(com[1]);
                            string Fprep1 = x.ToOrdinalWords();
                            string Fprep2 = y.ToOrdinalWords();
                            //case 4 and 5 or 5 and 6
                            if (x == y - 1)
                            {
                                if (Fprep1 == "first")
                                {
                                    Fprep1 = "Initial ";
                                    final = Fprep1 + "and " + Fprep2 + " combined reports ";
                                }
                                else { Fprep1 = Fprep1 + " and " + Fprep2 + " combined reports "; }
                            }
                            //case 1 - 5 or 3-10
                            if (x != y - 1)
                            {
                                if (Fprep1 == "first")
                                {
                                    Fprep1 = "Initial ";
                                    final = Fprep1 + "- " + Fprep2 + " combined reports";
                                }
                                else { Fprep1 = Fprep1 + " - " + Fprep2 + " combined reports "; }
                            }

                        }
                        break;
                    }
                case "French":
                    {
                        string cc = model.Prep.ToString();
                        string[] com = cc.Split('-');
                        var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

                        //case single number
                        if (com.Length == 1)
                        {
                            int x = Convert.ToInt32(model.Prep.ToString());
                            final = x.ToOrdinalWords(frenchCultureInfo);
                            final = final.Substring(0, 1).ToUpper() + final.Substring(1);
                            if (final == "Premier")
                            {
                                final = "Rapport initiaux ";
                            }
                            else { final = final + " rapport périodique "; }
                        }
                        //case combined number
                        if (com.Length > 1)
                        {
                            int x = Convert.ToInt32(com[0]);
                            int y = Convert.ToInt32(com[1]);
                            string Fprep1 = x.ToOrdinalWords(frenchCultureInfo);
                            string Fprep2 = y.ToOrdinalWords(frenchCultureInfo);
                            //case 4 and 5 or 5 and 6


                            if (Fprep1 == "premier")
                            {
                                Fprep1 = "initial ";
                                final = "Rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
                            }
                            else { final = "Rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }


                        }
                        break;
                    }
                case "Spanish":
                    {
                        final = SpanishNum1(model);

                        break;
                    }
            }

        
            return final;
        }
        public static string Reportnumber(OHCHRViewModel model)
        {
            string lang = language(model.lang_ID);
            string final = "";
          //  string country = getCount(model);
            string Country1 = getCountwithSrticle(model);
            //bool cat = false;
            //cat = categoryname(model);
            //if (cat == true)
            //{
                switch (lang)
                {
                    case "English":
                        {
                            string cc = model.Prep.ToString();
                            string[] com = cc.Split('-');

                            //case single number
                            if (com.Length == 1)
                            {
                            var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                            int x = Convert.ToInt32(model.Prep.ToString());
                                final = x.ToOrdinalWords(engCultureInfo);
                            if (final == "first")
                            {
                                final = "initial report " + Country1;
                            }
                            else { final = final + " periodic report " + Country1; }
                        }

                        //case combined number
                        if (com.Length > 1)
                            {
                                var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                                int x = Convert.ToInt32(com[0]);
                                int y = Convert.ToInt32(com[1]);
                                string Fprep1 = x.ToOrdinalWords(engCultureInfo);
                                string Fprep2 = y.ToOrdinalWords(engCultureInfo);
                            //case 4 and 5 or 5 and 6
                            if (x == y - 1)
                            {
                                if (Fprep1 == "first")
                                {
                                    Fprep1 = "initial ";
                                    final = Fprep1 + "and " + Fprep2 + " combined reports " + Country1;
                                }
                                else { final = Fprep1 + " and " + Fprep2 + " combined reports " + Country1; }
                            }
                            //case 1 - 5 or 3-10
                            if (x != y - 1)
                            {
                                if (Fprep1 == "first")
                                {
                                    Fprep1 = "initial ";
                                    final = Fprep1 + " to " + Fprep2 + " combined reports " + Country1;
                                }
                                else { final = Fprep1 + " to " + Fprep2 + " combined reports " + Country1; }
                            }

                        }
                        break;
                    }
                case "French":
                        {
                            string cc = model.Prep.ToString();
                            string[] com = cc.Split('-');
                            var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

                            //case single number
                            if (com.Length == 1)
                            {
                                int x = Convert.ToInt32(model.Prep.ToString());
                                final = x.ToOrdinalWords(frenchCultureInfo);
                            if (final == "premier")
                            {
                                final = "rapport initial " + Country1;
                            }
                            else { final = final + " rapport périodique " + Country1; }
                        }
                        //case combined number
                        if (com.Length > 1)
                            {
                                int x = Convert.ToInt32(com[0]);
                                int y = Convert.ToInt32(com[1]);
                                string Fprep1 = x.ToOrdinalWords(frenchCultureInfo);
                                string Fprep2 = y.ToOrdinalWords(frenchCultureInfo);
                            //case 4 and 5 or 5 and 6


                            if (Fprep1 == "premier")
                            {
                                Fprep1 = "rapport initial";
                                final = "rapport " + Country1 + " valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
                            }
                            else { final = "rapport " + Country1 + " valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }


                        }
                        break;
                    }
                case "Spanish":
                        {
                            final = SpanishNum(model);

                            break;
                        }
                }
            //}
            //if(cat==false)
            //{
            //    switch (lang)
            //    {
            //        case "English":
            //            {
            //                string cc = model.Prep.ToString();
            //                string[] com = cc.Split('-');

            //                //case single number
            //                if (com.Length == 1)
            //                {
            //                   // var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            //                    int x = Convert.ToInt32(model.Prep.ToString());
            //                    final = x.ToOrdinalWords();
            //                    final = final.Substring(0, 1).ToUpperInvariant() + final.Substring(1);
            //                    if (final == "First")
            //                    {
            //                        final = "Initial report ";
            //                    }
            //                    else { final = final + " periodic report "; }
            //                }

            //                //case combined number
            //                if (com.Length > 1)
            //                {
            //                    int x = Convert.ToInt32(com[0]);
            //                    int y = Convert.ToInt32(com[1]);
            //                    string Fprep1 = x.ToOrdinalWords();
            //                    string Fprep2 = y.ToOrdinalWords();
            //                    //case 4 and 5 or 5 and 6
            //                    if (x == y - 1)
            //                    {
            //                        if (Fprep1 == "first")
            //                        {
            //                            Fprep1 = "Initial ";
            //                            final = Fprep1 + "and " + Fprep2 + " combined reports ";
            //                        }
            //                        else { Fprep1 = Fprep1 + " and " + Fprep2 + " combined reports "; }
            //                    }
            //                    //case 1 - 5 or 3-10
            //                    if (x != y - 1)
            //                    {
            //                        if (Fprep1 == "first")
            //                        {
            //                            Fprep1 = "Initial ";
            //                            final = Fprep1 + "- " + Fprep2 + " combined reports";
            //                        }
            //                        else { Fprep1 = Fprep1 + " - " + Fprep2 + " combined reports "; }
            //                    }

            //                }
            //                break;
            //            }
            //        case "French":
            //            {
            //                string cc = model.Prep.ToString();
            //                string[] com = cc.Split('-');
            //                var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

            //                //case single number
            //                if (com.Length == 1)
            //                {
            //                    int x = Convert.ToInt32(model.Prep.ToString());
            //                    final = x.ToOrdinalWords(frenchCultureInfo);
            //                    final = final.Substring(0, 1).ToUpper() + final.Substring(1);
            //                    if (final == "Premier")
            //                    {
            //                        final = "Rapport initiaux ";
            //                    }
            //                    else { final = final + " rapport périodique "; }
            //                }
            //                //case combined number
            //                if (com.Length > 1)
            //                {
            //                    int x = Convert.ToInt32(com[0]);
            //                    int y = Convert.ToInt32(com[1]);
            //                    string Fprep1 = x.ToOrdinalWords(frenchCultureInfo);
            //                    string Fprep2 = y.ToOrdinalWords(frenchCultureInfo);
            //                    //case 4 and 5 or 5 and 6


            //                    if (Fprep1 == "premier")
            //                    {
            //                        Fprep1 = "initial ";
            //                        final = "Rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
            //                    }
            //                    else { final = "Rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }


            //                }
            //                break;
            //            }
            //        case "Spanish":
            //            {
            //                final = SpanishNum1(model);

            //                break;
            //            }
            //    }

            //}
            return final;
        }
        public static string ReportnumberW(OHCHRViewModel model)
        {
            string lang = language(model.lang_ID);
            string final = "";
            //  string country = getCount(model);
            string Country1 = getCountwithSrticle(model);
            //bool cat = false;
            //cat = categoryname(model);
            //if (cat == true)
            //{
            switch (lang)
            {
                case "English":
                    {
                        string cc = model.Prep.ToString();
                        string[] com = cc.Split('-');

                        //case single number
                        if (com.Length == 1)
                        {
                            var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                            int x = Convert.ToInt32(model.Prep.ToString());
                            final = x.ToOrdinalWords(engCultureInfo);
                            if (final == "first")
                            {
                                final = "initial report";
                            }
                            else { final = final + " periodic report"; }
                        }

                        //case combined number
                        if (com.Length > 1)
                        {
                            var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                            int x = Convert.ToInt32(com[0]);
                            int y = Convert.ToInt32(com[1]);
                            string Fprep1 = x.ToOrdinalWords(engCultureInfo);
                            string Fprep2 = y.ToOrdinalWords(engCultureInfo);
                            //case 4 and 5 or 5 and 6
                            if (x == y - 1)
                            {
                                if (Fprep1 == "first")
                                {
                                    Fprep1 = "initial ";
                                    final = Fprep1 + "and " + Fprep2 + " combined reports";
                                }
                                else { final = Fprep1 + " and " + Fprep2 + " combined reports"; }
                            }
                            //case 1 - 5 or 3-10
                            if (x != y - 1)
                            {
                                if (Fprep1 == "first")
                                {
                                    Fprep1 = "initial ";
                                    final = Fprep1 + " to " + Fprep2 + " combined reports";
                                }
                                else { final = Fprep1 + " to " + Fprep2 + " combined reports"; }
                            }

                        }
                        break;
                    }
                case "French":
                    {
                        string cc = model.Prep.ToString();
                        string[] com = cc.Split('-');
                        var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

                        //case single number
                        if (com.Length == 1)
                        {
                            int x = Convert.ToInt32(model.Prep.ToString());
                            final = x.ToOrdinalWords(frenchCultureInfo);
                            if (final == "premier")
                            {
                                final = "rapport initial";
                            }
                            else { final = final + " rapport périodique"; }
                        }
                        //case combined number
                        if (com.Length > 1)
                        {
                            int x = Convert.ToInt32(com[0]);
                            int y = Convert.ToInt32(com[1]);
                            string Fprep1 = x.ToOrdinalWords(frenchCultureInfo);
                            string Fprep2 = y.ToOrdinalWords(frenchCultureInfo);
                            //case 4 and 5 or 5 and 6


                            if (Fprep1 == "premier")
                            {
                                Fprep1 = "rapport initial";
                                final = "rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
                            }
                            else { final = "rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }


                        }
                        break;
                    }
                case "Spanish":
                    {
                        final = SpanishNumW(model);

                        break;
                    }
            }
            //}
            //if(cat==false)
            //{
            //    switch (lang)
            //    {
            //        case "English":
            //            {
            //                string cc = model.Prep.ToString();
            //                string[] com = cc.Split('-');

            //                //case single number
            //                if (com.Length == 1)
            //                {
            //                   // var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            //                    int x = Convert.ToInt32(model.Prep.ToString());
            //                    final = x.ToOrdinalWords();
            //                    final = final.Substring(0, 1).ToUpperInvariant() + final.Substring(1);
            //                    if (final == "First")
            //                    {
            //                        final = "Initial report ";
            //                    }
            //                    else { final = final + " periodic report "; }
            //                }

            //                //case combined number
            //                if (com.Length > 1)
            //                {
            //                    int x = Convert.ToInt32(com[0]);
            //                    int y = Convert.ToInt32(com[1]);
            //                    string Fprep1 = x.ToOrdinalWords();
            //                    string Fprep2 = y.ToOrdinalWords();
            //                    //case 4 and 5 or 5 and 6
            //                    if (x == y - 1)
            //                    {
            //                        if (Fprep1 == "first")
            //                        {
            //                            Fprep1 = "Initial ";
            //                            final = Fprep1 + "and " + Fprep2 + " combined reports ";
            //                        }
            //                        else { Fprep1 = Fprep1 + " and " + Fprep2 + " combined reports "; }
            //                    }
            //                    //case 1 - 5 or 3-10
            //                    if (x != y - 1)
            //                    {
            //                        if (Fprep1 == "first")
            //                        {
            //                            Fprep1 = "Initial ";
            //                            final = Fprep1 + "- " + Fprep2 + " combined reports";
            //                        }
            //                        else { Fprep1 = Fprep1 + " - " + Fprep2 + " combined reports "; }
            //                    }

            //                }
            //                break;
            //            }
            //        case "French":
            //            {
            //                string cc = model.Prep.ToString();
            //                string[] com = cc.Split('-');
            //                var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

            //                //case single number
            //                if (com.Length == 1)
            //                {
            //                    int x = Convert.ToInt32(model.Prep.ToString());
            //                    final = x.ToOrdinalWords(frenchCultureInfo);
            //                    final = final.Substring(0, 1).ToUpper() + final.Substring(1);
            //                    if (final == "Premier")
            //                    {
            //                        final = "Rapport initiaux ";
            //                    }
            //                    else { final = final + " rapport périodique "; }
            //                }
            //                //case combined number
            //                if (com.Length > 1)
            //                {
            //                    int x = Convert.ToInt32(com[0]);
            //                    int y = Convert.ToInt32(com[1]);
            //                    string Fprep1 = x.ToOrdinalWords(frenchCultureInfo);
            //                    string Fprep2 = y.ToOrdinalWords(frenchCultureInfo);
            //                    //case 4 and 5 or 5 and 6


            //                    if (Fprep1 == "premier")
            //                    {
            //                        Fprep1 = "initial ";
            //                        final = "Rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
            //                    }
            //                    else { final = "Rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }


            //                }
            //                break;
            //            }
            //        case "Spanish":
            //            {
            //                final = SpanishNum1(model);

            //                break;
            //            }
            //    }

            //}
            return final;
        }
        public static string SpanishNumW(OHCHRViewModel model)
        {
            string num1 = model.Prep.ToString();
            string[] str = num1.Split('-');
            string final = "";
            string Country1 = getCountwithSrticle(model);
            var spanishCultureInfo = CultureInfo.CreateSpecificCulture("es-es");
            if (str.Length == 1)
            {


                int num = Convert.ToInt32(model.Prep.ToString());
                if (num <= 10)
                {

                    final = num.ToOrdinalWords(spanishCultureInfo);
                    if (final == "primer")
                    {
                        final = "al informe inicial ";
                    }
                    else
                    {
                        final = "al " + final + " informe periódico ";
                    }
                }
                else
                {

                    string chr = num1.Last().ToString();
                    if (chr == "1" || chr == "3")
                    {
                        final = "al " + num1 + "\u1D49" + "\u02B3" + " informe periódico";
                    }
                    else
                    {
                        final = "al " + num1 + "\u1D52" + " informe periódico";
                    }

                }
            }
            if (str.Length > 1)
            {
                int x = Convert.ToInt32(str[0]);
                int y = Convert.ToInt32(str[1]);

                if (x <= 10 & y <= 10)
                {
                    string str1 = x.ToOrdinalWords(spanishCultureInfo);
                    string str2 = y.ToOrdinalWords(spanishCultureInfo);
                    if (x == y - 1)
                    {
                        final = "a los informes periódicos " + str1 + " y " + str2 + " combinados";
                        if (str1 == "primer")
                        {
                            final = "a los informes periódicos inicial y " + str2 + " combinados";
                        }

                    }
                    else
                    {
                        final = "a los informes periódicos " + str1 + " a " + str2 + " combinados";
                        if (str1 == "primer")
                        {
                            final = "a los informes periódicos inicial a " + str2 + " combinados";
                        }
                    }

                }
                if (x <= 10 & y > 10)
                {
                    string chr = str[0].Last().ToString();
                    string final1 = "";
                    string final2 = "";
                    final1 = str[0] + "\u1D52";
                    final2 = str[1] + "\u1D52";

                    if (x == y - 1)
                    {
                        final = "a los informes periódicos " + final1 + " y " + final2 + " combinados";
                    }
                    else
                    {
                        final = "a los informes periódicos " + final1 + " a " + final2 + " combinados";
                    }
                }
                if (x > 10 & y > 10)
                {
                    string chr = str[0].Last().ToString();
                    string final1 = "";
                    string final2 = "";
                    final1 = str[0] + "\u1D52";
                    final2 = str[1] + "\u1D52";

                    if (x == y - 1)
                    {
                        final = "a los informes periódicos " + final1 + " y " + final2 + " combinados";
                    }
                    else
                    {
                        final = "a los informes periódicos " + final1 + " a " + final2 + " combinados";
                    }
                }

            }
            return final;

        }
        public static string SpanishNum1(OHCHRViewModel model)
        {
          
            


            string num1 = model.Prep.ToString();
            string[] str = num1.Split('-');
            string final = "";
            var spanishCultureInfo = CultureInfo.CreateSpecificCulture("es-ES");
            if (str.Length == 1)
            {


                int num = Convert.ToInt32(model.Prep.ToString());


                final = Toordinalspanish(model.Prep.ToString());
                final = final.Substring(0, 1).ToUpper() + final.Substring(1);
                if (final == "Primero")
                {
                    final = "Informes iniciales";
                }
                else
                {
                    final = Toordinalspanish(model.Prep.ToString());
                    final = final.Substring(0, 1).ToUpper() + final.Substring(1);
                    final = final + "s" + " informes periódicos ";
                }


                //if (num <= 10)
                //{
                //    final = num.ToOrdinalWords(spanishCultureInfo);
                //    final = final.Substring(0, 1).ToUpper() + final.Substring(1);
                //    if (final == "Primer")
                //    {
                //        final = "Informe iniciales";
                //    }
                //    else
                //    {
                //        final = num.ToOrdinalWords(spanishCultureInfo) + " informe periódico ";
                //    }
                //}
                //else
                //{

                //    string chr = num1.Last().ToString();
                //    if (chr == "1" || chr == "3")
                //    {
                //        final = num1 + "\u1D49" + "\u02B3" + " informe periódico";
                //        final = final.Substring(0, 1).ToUpper() + final.Substring(1);
                //    }
                //    else
                //    {
                //        final = num1 + "\u1D52" + " informe periódico";
                //        final = final.Substring(0, 1).ToUpper() + final.Substring(1);
                //    }

                //}
            }
            if (str.Length > 1)
            {
                int x = Convert.ToInt32(str[0]);
                int y = Convert.ToInt32(str[1]);

                if (x <= 10 & y <= 10)
                {
                    string str1 = Toordinalspanish(str[0]);
                    string str2 = Toordinalspanish(str[1]);
                    if (x == y - 1)
                    {
                        final = "Informes periódicos " + str1 + "s" + " y " + str2 + "s" + " combinados ";
                        if (str1 == "primer")
                        {
                            final = "Informes periódicos inicial y " + str2 + "s" + " combinados ";
                        }

                    }
                    else
                    {

                        final = "Informes periódicos " + str1 + "s" + " a " + str2 + "s" + " combinados ";
                        if (str1 == "primer")
                        {
                            final = "Informes periódicos inicial a " + str2 + "s" + " combinados ";
                        }
                    }

                }
                if (x <= 10 & y > 10)
                {
                    string chr = str[0].Last().ToString();
                    string final1 = "";
                    string final2 = "";
                    final1 = str[0] + "\u1D52";
                    final2 = str[1] + "\u1D52";

                    if (x == y - 1)
                    {
                        final = "Informes periódicos " + final1 + " y " + final2 + " combinados ";
                    }
                    else
                    {
                        final = "Informes periódicos " + final1 + " a " + final2 + " combinados ";
                    }
                }
                if (x > 10 & y > 10)
                {
                    string chr = str[0].Last().ToString();
                    string final1 = "";
                    string final2 = "";
                    final1 = str[0] + "\u1D52";
                    final2 = str[1] + "\u1D52";

                    if (x == y - 1)
                    {
                        final = "Informes periódicos " + final1 + " y " + final2 + " combinados ";
                    }
                    else
                    {
                        final = "Informes periódicos " + final1 + " a " + final2 + " combinados ";
                    }
                }
            }
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
        public static string SpanishNum(OHCHRViewModel model)
        {
            string num1 = model.Prep.ToString();
            string[] str = num1.Split('-');
            string final = "";
             string Country1 = getCountwithSrticle(model);
            var spanishCultureInfo = CultureInfo.CreateSpecificCulture("es-es");
            if (str.Length == 1)
            {
                

                int num = Convert.ToInt32(model.Prep.ToString());

                if (num <= 10)
                {
                    final = num.ToOrdinalWords(spanishCultureInfo);
                    if (final == "primer")
                    {
                        final = "al informe inicial " + Country1;
                    }
                    else
                    {
                        final = "al " + final + " informe periódico " + Country1;
                    }
                }
                else
                {

                    string chr = num1.Last().ToString();
                    if (chr == "1" || chr == "3")
                    {
                        final = "al " + num1 + "\u1D49" + "\u02B3" + " informe periódico " + Country1;
                    }
                    else
                    {
                        final = "al " + num1 + "\u1D52" + " informe periódico " + Country1;
                    }

                }
            }
            if (str.Length > 1)
            {
                int x = Convert.ToInt32(str[0]);
                int y = Convert.ToInt32(str[1]);

                if (x <= 10 & y <= 10)
                {
                    string str1 = x.ToOrdinalWords(spanishCultureInfo);
                    string str2 = y.ToOrdinalWords(spanishCultureInfo);
                    if (x == y - 1)
                    {
                        final = "a los informes periódicos " + str1 + " y " + str2 + " combinados " + Country1;
                        if (str1 == "primer")
                        {
                            final = "a los informes periódicos primero y " + str2 + " combinados " + Country1;
                        }

                    }
                    else
                    {
                        final = "a los informes periódicos " + str1 + " a " + str2 + " combinados " + Country1;
                        if (str1 == "primer")
                        {
                            final = "a los informes periódicos primero a " + str2 + " combinados " + Country1;
                        }
                    }

                }
                if (x <= 10 & y > 10)
                {
                    string chr = str[0].Last().ToString();
                    string final1 = "";
                    string final2 = "";
                    final1 = str[0] + "\u1D52";
                    final2 = str[1] + "\u1D52";

                    if (x == y - 1)
                    {
                        final = "a los informes periódicos " + final1 + " y " + final2 + " combinados " + Country1;
                    }
                    else
                    {
                        final = "a los informes periódicos " + final1 + " a " + final2 + " combinados " + Country1;
                    }
                }
                if (x > 10 & y > 10)
                {
                    string chr = str[0].Last().ToString();
                    string final1 = "";
                    string final2 = "";
                    final1 = str[0] + "\u1D52";
                    final2 = str[1] + "\u1D52";

                    if (x == y - 1)
                    {
                        final = "a los informes periódicos " + final1 + " y " + final2 + " combinados " + Country1;
                    }
                    else
                    {
                        final = "a los informes periódicos " + final1 + " a " + final2 + " combinados " + Country1;
                    }
                }

            }
            return final;

        }
        public static string Sessionnum(OHCHRViewModel model)
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
                            final = num.ToOrdinalWords(spanishCultureInfo);
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
            }

            final = char.ToUpper(final[0]) + final.Substring(1);
            return final;
        }
        public static string[] SortVirsion(string [] list)
        {
            int pos = Array.IndexOf(list, "inglés");
            if (pos > -1)
            {
                int lastindex = list.Length - 1;


                var last = list.Last();
               // var first = list.First();

                if (pos != lastindex)
                {
                    return list;
                }
                if (pos == lastindex & list.Length == 1)
                {
                    return list;
                }
                else 
                {
                    string rep = list[0];
                    list[0] = "inglés";
                    list[pos] = rep;
                    return list;
                }
                
            }
            return list;
        }

        public static string getVerisons(OHCHRViewModel model)
        {
            string [] s = model.version1.ToArray();
            //  s = SortVirsion(s);
            s = s.OrderBy(q => q).ToArray();
            for (int i = 0; i < s.Length; i++)
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
                           Fvirs = s[0] + " y " + s[1] + " únicamente";
                       }
                       if (s.Length == 3)
                       {
                           Fvirs = s[0] + ", " + s[1] + " y " + s[2] + " únicamente";
                       }
                       if (s.Length == 4)
                       {
                           Fvirs = s[0] + ", " + s[1] + ", " + s[2] + " y " + s[3] + " únicamente";
                       }
                       if (s.Length == 5)
                       {
                           Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + " y " + s[4] + " únicamente";
                       }

                       if (s.Length == 6)
                       {
                           Fvirs = s[0] + ", " + s[1] + ", " + s[2] + ", " + s[3] + ", " + s[4] + " y " + s[5] + " únicamente";
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
        public static bool categoryname(OHCHRViewModel model)
        {

            gdgs1Entities db1 = new gdgs1Entities();
            var item = db1.categories.FirstOrDefault(p => p.ID == model.Cat);
            string cat = item.Name.ToString();
            if (cat == "Country Report" || cat == "Country Report -Optional Procedure")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string SessionDate(string sdate,string edate, OHCHRViewModel model)
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

        public static string formatdate(string date, OHCHRViewModel model)
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
                if(d[1]=="1")
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
        public static string getSym(OHCHRViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.final_temp.FirstOrDefault(p => p.categories_ID == model.Cat);
            string sym = item.Symbole.ToString();
            return sym;
        }

        public static string getDist(OHCHRViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.distrbutions.FirstOrDefault(p => p.ID.ToString() == model.dist);
            string dist = item.Name.ToString();
            return dist;
        }

        public static string getIso(OHCHRViewModel model)
        { 
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.countries.FirstOrDefault(p => p.languages_ID == model.lang_ID & p.ID==model.count_ID);
            if (item == null)
            {
                return "";
            }
            else
            {
                string iso = item.ISO.ToString();
                return iso;
            }
        }
        public static string TempName(OHCHRViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var temp = db1.final_temp.Where(a => a.categories_ID == model.Cat).FirstOrDefault();
            if (temp == null)
            {
                return "Worng Template";
            }
            else
            {
                string lang = language(model.lang_ID);
                lang = lang.Substring(0,1);
                string tempname = temp.Name.ToString() + lang;
                return tempname;
            }
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

        public static string getCount(OHCHRViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.countries.Where(x => x.ID == model.count_ID).FirstOrDefault();
            string Cname = count.Short_Name.ToString();
            string Article = count.Article.ToString(); bool cat = false;
            cat = categoryname(model);
            var item = db1.languages.Where(p => p.ID == model.lang_ID).FirstOrDefault();
            string lang = item.Lang_Name.ToString();

            if (cat == true)
            {
                switch (lang)
                {
                    case "English":
                        {

                            break;
                        }
                    case "French":
                        {
                            Cname = Article + " " + Cname;
                            break;
                        }
                    case "Spanish":
                        {
                            Cname = Article + " " + Cname;
                            break;
                        }
                }
            }
            else
            {
                return Cname;
            }


            return Cname;
        }
        public static string getCountW(OHCHRViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.countries.Where(x => x.ID == model.count_ID).FirstOrDefault();
            string Cname = count.Short_Name.ToString();
            return Cname;
        }
        public static string getCountWD(OHCHRViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.countries.Where(x => x.ID == model.count_ID).FirstOrDefault();
            string Cname = count.Short_Name.ToString();
            string Article = count.Article1.ToString();

            bool cat = false;
            cat = categoryname(model);
            var item = db1.languages.Where(p => p.ID == model.lang_ID).FirstOrDefault();
            string lang = item.Lang_Name.ToString();

            if (cat == true)
            {
                switch (lang)
                {
                    case "English":
                        {

                            break;
                        }
                    case "French":
                        {
                            if (Article == "No Article")
                            {
                                break;
                            }
                            else
                            {
                                Cname = Article + " " + Cname;
                                break;
                            }

                        }
                    case "Spanish":
                        {
                            if (Article == "No Article")
                            {
                                break;
                            }
                            else
                            {
                                Cname = Article + " " + Cname;
                                break;
                            }
                        }
                }
            }
            else
            {
                return Cname;
            }


            return Cname;
        }
        public static string getCountwithSrticle(OHCHRViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.countries.Where(x => x.ID == model.count_ID).FirstOrDefault();
            string Cname = count.Short_Name.ToString();
            string Article = count.Article.ToString();
            return Article + " "+Cname;
        }
        public static string language(int id)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var language = db1.languages.Where(x => x.ID == id).FirstOrDefault();
            string lname = language.Lang_Name.ToString(); 
            return lname;
        }

        [HttpPost]
        public ActionResult Add(OHCHRViewModel model)
        {
            return View(model);
        }
        [HttpPost]
        public ActionResult Communication(OHCHRViewModel model)
        {
            return View(model);
        }
        [HttpPost]
        public ActionResult Report(OHCHRViewModel model)
        {
            return View(model);
        }

        //Decide what type of View to show Addendum or normal report or Communication
        public string TypeOfView(int category)
        {
            var temp = db.final_temp.Where(c => c.categories_ID == category).FirstOrDefault();
            string sym = temp.Symbole.ToString();
            sym = sym.Replace("/", "");
            bool add = sym.Contains("$");
            bool comm = sym.Contains("*");

            if (add == true)
            {
                return "Add";
            }
            if (comm == true)
            {
                return "Communication";
            }
            else return "Report";
        }


    }
}