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
using System.Text.RegularExpressions;
using Aspose.Words;
using System.Collections;
using Aspose.Words.Fields;

namespace IdentitySample.Controllers
{
    public class OhchrSymbController : Controller
    {

        static Assembly g_assembly;
        static DocX g_document;
        private gdgs1Entities db = new gdgs1Entities();

        // GET: OhchrSymb
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FindTemplate(UnogViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();
            UnogViewModel Newmodel = new UnogViewModel();
            //Filtering possible Matches according to Symbole structure
            string sym = model.Sym.ToString();
            int Count = sym.Split('/').Count();
            int i = 0;
            int index = 0;

            int numofmatches = 0;
            //string Matches ="";


            List<UnogViewModel> List = new List<UnogViewModel>();

            List = db1.final_temp.Where(p => p.Count == Count).Select(Item => new UnogViewModel
            {
                tempname = Item.Name,
                structure = Item.Symbole,
                regx = Item.Reg,
            }).ToList();

            foreach (var l in List)
            {

                var pattern = new Regex(@l.regx.ToString());
                if (pattern.IsMatch(sym.ToString()))
                {
                    index = i;
                    numofmatches = numofmatches + 1;
                }
                i++;
            }
            if (numofmatches > 1)
            {
                string s = List[index].structure.ToString();
                model.structure = s;
                if (model.version1 != null)
                {
                    model.ver = string.Join(",", model.version1.ToArray());
                    //model.ver = model.version1.ToString();
                }
                return RedirectToAction("Verify", model);
            }
            if (numofmatches == 1)
            {
                string x = List[index].tempname.ToString();
                string xx = List[index].structure.ToString();
                model.structure = xx;
                model.tempname = x;
                var lang = db1.languages.Where(p => p.ID == model.lang_ID).FirstOrDefault();
                string name = lang.Lang_Name.ToString().Substring(0, 1);
                model.tempname = model.tempname + name;
                if (model.version1 != null)
                {
                    model.ver = string.Join(",", model.version1.ToArray());
                   // model.ver = model.version1.ToString();
                }
                model.datestring = model.date.ToString();

                if (!String.IsNullOrEmpty(model.Sdate.ToString()))
                {
                    model.sdatestring = model.Sdate.ToString();
                }
                if (!String.IsNullOrEmpty(model.Edate.ToString()))
                {
                    model.edatestring = model.Edate.ToString();
                }
                return RedirectToAction("Generate", model);
            }

            else
            {
                return RedirectToAction("WrongTemplate");

            }

        }
        public ActionResult WrongTemplate()
        {
            return View();

        }
        [HttpGet]
        public ActionResult Verify(UnogViewModel model)
        {
            model.datestring = model.date.ToString();

            if (!String.IsNullOrEmpty(model.Sdate.ToString()))
            {
                model.sdatestring = model.Sdate.ToString();
            }
            if (!String.IsNullOrEmpty(model.Edate.ToString()))
            {
                model.edatestring = model.Edate.ToString();
            }
            return View(model);

        }

        [HttpPost]
        public ActionResult VerifyF(UnogViewModel model)
        {
            string fname = model.tempname.ToString();
            var lang = db.languages.Where(p => p.ID == model.lang_ID).FirstOrDefault();
            string name = lang.Lang_Name.ToString().Substring(0, 1);
            fname = fname + name;


            g_assembly = Assembly.GetExecutingAssembly();

             string source = Server.MapPath(Path.Combine("/", "GDGS/IN/" + fname + ".docx"));
             string Dest = Server.MapPath(Path.Combine("/", "GDGS/OUT/" + fname + ".docx"));

              //string source = Server.MapPath(Path.Combine("/", "IN/" + fname + ".docx"));
            // string Dest = Server.MapPath(Path.Combine("/", "OUT/" + fname + ".docx"));

            g_document = DocX.Load(source);
            g_document = CreateDocA(DocX.Load(source), model);
           
            g_document.SaveAs(Dest);
            g_document.AddCoreProperty("dc:title", model.Sym.ToString());
            g_document.Save();
            return RedirectToAction("download", "OhchrSymb", new { name = fname });

        }
        public ActionResult Generate(UnogViewModel model)
        {
            //get temp name from reg expression plus lang plus symb
            string fname = model.tempname.ToString();
            g_assembly = Assembly.GetExecutingAssembly();

             string source = Server.MapPath(Path.Combine("/", "GDGS/IN/" + fname + ".docx"));
             string Dest = Server.MapPath(Path.Combine("/", "GDGS/OUT/" + fname + ".docx"));

        //  string source = Server.MapPath(Path.Combine("/", "IN/" + fname + ".docx"));
         //  string Dest = Server.MapPath(Path.Combine("/", "OUT/" + fname + ".docx"));


            string tempname = fname.Remove(fname.Length - 1);
            model.tempname = tempname;
            g_document = DocX.Load(source);
            g_document = CreateDocA(DocX.Load(source), model);
          
            g_document.SaveAs(Dest);
            g_document.AddCoreProperty("dc:title", model.Sym.ToString());
            g_document.Save();
            
            return RedirectToAction("download", "OhchrSymb", new { name = fname });

        }
        public ActionResult download(string name)
        {
            string source1 = Server.MapPath(Path.Combine("/", "GDGS/OUT/" + name + ".docx"));
            //string source1 = Server.MapPath(Path.Combine("/", "OUT/" + name + ".docx"));
            //Open document

            Document doc = new Document(source1);

          

            //Get collection of FiledStart nodes

            ArrayList propertyStarts = new ArrayList();

            NodeCollection starts = doc.GetChildNodes(NodeType.FieldStart, true);

            foreach (FieldStart start in starts)

            {

                if (start.FieldType == FieldType.FieldDocProperty)

                {

                    propertyStarts.Add(start);

                }

            }

            //For each DOCUMENTPROPERTY Field Start

            foreach (FieldStart start in propertyStarts)

            {

                Node currentNode = start;

                Node fieldSeparator = null;

                //Remove field code

                while (currentNode.NodeType != NodeType.FieldSeparator)

                {

                    currentNode = currentNode.NextSibling;

                    currentNode.PreviousSibling.Remove();

                }

                fieldSeparator = currentNode;

                //Move to Field End

                while (currentNode.NodeType != NodeType.FieldEnd)

                {

                    currentNode = currentNode.NextSibling;

                }

                //Remove field separator

                fieldSeparator.Remove();

                //Romove field end

                currentNode.Remove();

            }

            //Save document

            doc.Save(source1);

            return File(Url.Content("/GDGS/OUT/" + name + ".docx"), "text/plain", name + ".docx");
           // return File(Url.Content("/OUT/" + name + ".docx"), "text/plain", name + ".docx");
        }

        public static string formatdate(string date, UnogViewModel model)
        {
            //gdgs1Entities db1 = new gdgs1Entities();

            //var item = db1.languages.FirstOrDefault(p => p.ID == model.lang_ID);
            //string lang = item.Lang_Name.ToString();

            //string final = "day month year";
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
            //    string[] d = date.Split('/');
            //    int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
            //    var frenchCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            //    string month = frenchCultureInfo.DateTimeFormat.GetMonthName(convert[0]);
            //    final = d[1] + " " + month + " " + d[2];
            //}
            //return final;
            gdgs1Entities db1 = new gdgs1Entities();
            
            var item = db1.languages.FirstOrDefault(p => p.ID == model.lang_ID);
            string lang = item.Lang_Name.ToString();

            string final = "day month year";
            if (lang == "French")
            {
                string[] d = date.Split('/');
                
                int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
                var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");
                string month = frenchCultureInfo.DateTimeFormat.GetMonthName(convert[1]);
                string day = d[0].Substring(0,1);
                if (day == "0")
                {
                    d[0] = d[0].Remove(0, 1);
                }
               
                if (d[0] == "1")
                {
                    d[0] = d[0] + "\u1D49" + "\u02B3";
                }
                final = d[0] + " " + month + " " + d[2];
            }
            else if (lang == "Spanish")
            {
                string[] d = date.Split('/');
                int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
                var frenchCultureInfo = CultureInfo.CreateSpecificCulture("es-es");
                string month = frenchCultureInfo.DateTimeFormat.GetMonthName(convert[1]);
                string day = d[0].Substring(0, 1);
                if (day == "0")
                {
                    d[0] = d[0].Remove(0, 1);
                }
                final = d[0] + " de " + month + " de " + d[2];
            }
            else
            {
                string[] d = date.Split('/');
                int[] convert = Array.ConvertAll<string, int>(d, int.Parse);
                var frenchCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                string month = frenchCultureInfo.DateTimeFormat.GetMonthName(convert[1]);
                string day = d[0].Substring(0, 1);
                if (day == "0")
                {
                    d[0] = d[0].Remove(0, 1);
                }
                final = d[0] + " " + month + " " + d[2];
            }
            return final;
        }
        public static string getDist(UnogViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.distrbutions.FirstOrDefault(p => p.ID.ToString() == model.dist);
            string dist = item.Name.ToString();
            return dist;
        }
        //check this
        public static string Olanguage(int id)
        {
            gdgs1Entities db1 = new gdgs1Entities();
            var language = db1.languages.Where(x => x.ID == id).FirstOrDefault();
            string lname = language.Lang_Name.ToString();
            return lname;

        }
        //check this
        public static string Olanguage1(int id)
        {
            gdgs1Entities db1 = new gdgs1Entities();
            var language = db1.olanguages.Where(x => x.ID == id).FirstOrDefault();
            string lname = language.Name.ToString();
            return lname;

        }

        public static string Tlanguage(int id)
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
        public static string SessionDate(string sdate, string edate, UnogViewModel model)
        {
            string final = "";
            string[] s = sdate.Split(' ');
            string[] e = edate.Split(' ');
            string lang = Olanguage(model.lang_ID);
            if (sdate == edate)
            {
                final = sdate;
                return final;
            }
            else {
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
            }
            return final;
        }
        public static string Sessionnum(UnogViewModel model)
        {
            string lang = Olanguage(model.lang_ID);
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

            final = char.ToUpper(final[0]) + final.Substring(1);
            return final;
        }
        public static DocX CreateDocA(DocX template, UnogViewModel model)
        {
            string sym = model.Sym.ToString();
            string[] SymStr = sym.Split('/');
            string[] sym1 = SymStr.Skip(1).ToArray();
            string Fsym = String.Join("/", sym1);
            int reppos = 0;
            int repposcom = 0;
            int countpos = 0;


            //Create Date for Header
            string Fdate = model.datestring;
            string[] d = Fdate.Split(' ');
            Fdate = d[0];
            Fdate = formatdate(Fdate, model);
            //Create Distrbution for Header
            string Fdist = getDist(model);
            //document Language "Section"
            string Ftlang = "";
           // Ftlang = Tlanguage(model.lang_ID);
            //ORiginal Language
            string Folang = "";
            
             Folang = Tlanguage(model.lang_ID);
            string FAuthor = "";
            //Typist
            if (!String.IsNullOrEmpty(model.Author))
            {
                FAuthor = model.Author.ToString();
            }
            //Agenda Title
            string Fatitle = "[Title]";
            if (!String.IsNullOrEmpty(model.AgendaItem))
            {
                Fatitle = model.AgendaItem.ToString();
            }
            //Session Title
            string Fstitle = "[Title]";
            if (!String.IsNullOrEmpty(model.STitle))
            {
                Fstitle = model.STitle.ToString();
            }
            //Session Dates
            //Session Dates
            string Fsdate = "[Start-End Dates]";
            if (!String.IsNullOrEmpty(model.sdatestring)  & !String.IsNullOrEmpty(model.edatestring))
            {
                
                string sdate = model.sdatestring;
                string[] sd = sdate.Split(' ');
                sdate = sd[0];
                sdate = formatdate(sdate, model);

                string edate = model.edatestring;
                string[] ed = edate.Split(' ');
                edate = ed[0];
                edate = formatdate(edate, model);


                Fsdate = SessionDate(sdate, edate, model);
            }

            //Agenda Number
            string Fanum = "[NUMBER]";
            if (!String.IsNullOrEmpty(model.AgendaNum))
            {
                Fanum = model.AgendaNum.ToString();
            }
            //Session Numer
            string Fsnum = "[NUMBER]";
            if (!String.IsNullOrEmpty(model.SNum))
            {
                Fsnum = Sessionnum(model);
            }

            ///
            //Report num and count

            string structure = model.structure.ToString();
            string[] str = structure.Split('/');
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == "#")
                {
                    reppos = i;
                }
                if (str[i] == "ISO")
                {
                    countpos = i;
                }
                if (str[i] == "R.#")
                {
                    repposcom = i;
                }

            }

            string Fprep = "";
            string FprepW = "";
            string FprepWC = "";
            string FprepWS = "";
            string Fpreps = "";
            if (reppos != 0 && countpos != 0)
            {
                Fprep = Reportnumber(model, SymStr[reppos], model.lang_ID, SymStr[countpos]);
                Fpreps = Reportnumbers(model, SymStr[reppos], model.lang_ID, SymStr[countpos]);
                FprepW = ReportnumberW(model, SymStr[reppos], model.lang_ID, SymStr[countpos]);
                FprepWC = ReportnumberWC(model, SymStr[reppos], model.lang_ID, SymStr[countpos]);
                FprepWS = ReportnumberWS(model, SymStr[reppos], model.lang_ID, SymStr[countpos]);


            }
            if (repposcom != 0 && countpos != 0)
            {
                string x = SymStr[repposcom];
                int start = x.IndexOf('.');
                start = start + 1;
                string prep = x.Substring(start);

                Fprep = Reportnumber(model, prep, model.lang_ID, SymStr[countpos]);
                FprepW = ReportnumberW(model, prep, model.lang_ID, SymStr[countpos]);
                FprepWC = ReportnumberWC(model, prep, model.lang_ID, SymStr[countpos]);
                Fpreps = Reportnumbers(model, prep, model.lang_ID, SymStr[countpos]);
                FprepWS = ReportnumberWS(model, prep, model.lang_ID, SymStr[countpos]);

            }

            string fCount = "";
            string fCountW = "";
            string fCountWD = "";

            if (countpos != 0)
            {
                fCount = getCountvalue(SymStr[countpos], model);
                fCountW = getCountvalueW(SymStr[countpos], model);
                fCountWD = getCountvalueWD(SymStr[countpos], model);

            }


            //QR code and Bar code

            string lang = getlanguageQR(model.lang_ID);



            string url = "http://undocs.org/m2/QRCode.ashx?DS=" + Fsym + "&Size=2&Lang=" + lang;
            using (var client = new WebClient())
            {
                //var content = client.DownloadData("https://api.qrserver.com/v1/create-qr-code/?size=66x66&data=http://undocs.org/fr/A/HRC/70");
                //var content = client.DownloadData("http://undocs.org/m2/QRCode.ashx?DS=A/HRC/70&Size=2&Lang=F");
                var content = client.DownloadData(url);

                using (var str1 = new MemoryStream(content))
                {

                    if (lang == "A")
                    {
                        Image image = template.AddImage(str1);
                        Picture p = image.CreatePicture();
                        Footer f = template.Footers.first;
                        Table t = f.Tables[0];
                        //t.Rows[0].Cells[1].Paragraphs.First().AppendPicture(pR);
                        t.Rows[0].Cells[0].Paragraphs.First().AppendPicture(p);
                    }
                    else
                    {
                        Image image = template.AddImage(str1);
                        Picture p = image.CreatePicture();
                        Footer f = template.Footers.first;
                        Table t = f.Tables[0];
                        //t.Rows[0].Cells[1].Paragraphs.First().AppendPicture(pR);
                        t.Rows[0].Cells[1].Paragraphs.First().AppendPicture(p);
                    }
                }
            }

            //create barcode
            string Fbar = "";
            //Fbar = model.Gdoc.ToString();
           // Fbar = "*" + Fbar + "*";

            //creat gdoc
            string gdoc = "";
           // gdoc = gdoc.Insert(2, "-");

            //creat gdoc footer
            string Fgdocf = gdoc;
            //no translation with no versions English only
            if (model.ver == null & model.ntv==false)
            {

                int deleteStart = 0;
                int deleteEnd = 0;

                //Get the array of the paragraphs containing the start and end catches
                for (int i = 0; i < template.Paragraphs.Count; i++)
                {
                    if (template.Paragraphs[i].Text.Contains("Original"))
                        deleteStart = i;
                    if (template.Paragraphs[i].Text.Contains("olang"))
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
            //translation with no versions (original English)
            if (model.ver == null & model.ntv == true)
            {

                int deleteStart = 0;
                int deleteEnd = 0;

                //Get the array of the paragraphs containing the start and end catches
                for (int i = 0; i < template.Paragraphs.Count; i++)
                {
                    if (template.Paragraphs[i].Text.Contains("virs"))
                        deleteStart = i;
                    if (template.Paragraphs[i].Text.Contains("virs"))
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
            //Create Virsions for Header
            string Fvirs = "";

          
                Fvirs = getVerisons(model);
           
            DateTime xxx = DateTime.Now;

            string[] info1 = info(model);

            template.AddCustomProperty(new CustomProperty("sym1", Fsym));
            template.AddCustomProperty(new CustomProperty("symh", sym));
            template.AddCustomProperty(new CustomProperty("dist", Fdist));
            template.AddCustomProperty(new CustomProperty("date", Fdate));
            template.AddCustomProperty(new CustomProperty("sdate", Fsdate));
            template.AddCustomProperty(new CustomProperty("virs", Fvirs));
            template.AddCustomProperty(new CustomProperty("snum", Fsnum));
            template.AddCustomProperty(new CustomProperty("anum", Fanum));
            template.AddCustomProperty(new CustomProperty("count", fCount));
            template.AddCustomProperty(new CustomProperty("countw", fCountW));
            template.AddCustomProperty(new CustomProperty("countwd", fCountWD));
            template.AddCustomProperty(new CustomProperty("gdoc", gdoc));
            template.AddCustomProperty(new CustomProperty("gdocf", Fgdocf));
            template.AddCustomProperty(new CustomProperty("tlang", Ftlang));
            template.AddCustomProperty(new CustomProperty("atitle", Fatitle));
            template.AddCustomProperty(new CustomProperty("stitle", Fstitle));
            template.AddCustomProperty(new CustomProperty("prep", Fprep));
            template.AddCustomProperty(new CustomProperty("preps", Fpreps));
            template.AddCustomProperty(new CustomProperty("prepw", FprepW));
            template.AddCustomProperty(new CustomProperty("prepws", FprepWS));
            template.AddCustomProperty(new CustomProperty("prepwc", FprepWC));
            template.AddCustomProperty(new CustomProperty("olang", Folang));
            template.AddCustomProperty(new CustomProperty("Author", FAuthor));
            template.AddCustomProperty(new CustomProperty("bar", Fbar));
            template.AddCustomProperty(new CustomProperty("Date-Generated", xxx));
            template.AddCustomProperty(new CustomProperty("Title", sym));
            template.AddCustomProperty(new CustomProperty("Org", "OHCHR"));
            template.AddCustomProperty(new CustomProperty("Entity", info1[0]));
            template.AddCustomProperty(new CustomProperty("doctype", info1[1]));
            template.AddCustomProperty(new CustomProperty("category", info1[2]));


          

            return template;
        }
        public static string[] info(UnogViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();
            var item0 = db1.final_temp.FirstOrDefault(p => p.Name == model.tempname);
            var item = db1.categories.FirstOrDefault(p => p.ID == item0.categories_ID);
            var item1 = db1.doc_type.FirstOrDefault(p => p.ID == item.type_ID);
            var item2 = db1.committees.FirstOrDefault(p => p.ID == item1.committee_ID);
            string[] info = new string[3];
            info[0] = item.Name.ToString();
            info[1] = item1.Name.ToString();
            info[2] = item2.Name.ToString();
            return info;
        }
        public static string getCountvalue(string iso, UnogViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.countries.Where(x => x.languages_ID == model.lang_ID && x.ISO == iso).FirstOrDefault();
            string Cname = count.Short_Name.ToString();
            string Article = count.Article.ToString();
            //bool cat = false;
            //cat = categoryname(model);
            var item = db1.languages.Where(p => p.ID == model.lang_ID).FirstOrDefault();
            string lang = item.Lang_Name.ToString();

            //if (cat == true)
            //{
                switch (lang)
                {
                    case "English":
                        {

                            break;
                        }
                    case "French":
                        {
                        if (Article == "l'")
                        {
                            Cname = Article + Cname;
                        }
                        else
                        {
                            Cname = Article + " " + Cname;
                        }
                        break;
                        }
                    case "Spanish":
                        {
                            Cname = Article + " " + Cname;
                            break;
                        }
                     default:
                        {
                            break;
                        }
                }
            //}
            //else
            //{
            //    return Cname;
            //}


            return Cname;
        }
        public static string getCountvalueWD(string iso, UnogViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.countries.Where(x => x.languages_ID == model.lang_ID && x.ISO == iso).FirstOrDefault();
            string Cname = count.Short_Name.ToString();
            string Article = count.Article1.ToString();

            //bool cat = false;
            //cat = categoryname(model);
            var item = db1.languages.Where(p => p.ID == model.lang_ID).FirstOrDefault();
            string lang = item.Lang_Name.ToString();

            //if (cat == true)
            //{
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
                            if (Article == "l'")
                            {
                                Cname = Article + Cname;
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
                    default:
                        {
                            break;
                        }
                }
            //}
            //else
            //{
            //    return Cname;
            //}


            return Cname;
        }
        public static string getCountvalueW(string iso, UnogViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.countries.Where(x => x.languages_ID == model.lang_ID && x.ISO == iso).FirstOrDefault();
            string Cname = count.Short_Name.ToString();
            string Article = count.Article.ToString();
            //bool cat = false;
            //cat = categoryname(model);
            // var item = db1.languages.Where(p => p.ID == model.lang_ID).FirstOrDefault();
            // string lang = item.Lang_Name.ToString();

            //if (cat == true)
            //{
            //    switch (lang)
            //    {
            //        case "English":
            //            {

            //                break;
            //            }
            //        case "French":
            //            {
            //                Cname = Article + " " + Cname;
            //                break;
            //            }
            //        case "Spanish":
            //            {
            //                Cname = Article + " " + Cname;
            //                break;
            //            }
            //    }
            //}
            //else
            //{
            //    return Cname;
            //}


            return Cname;
        }
        public static string getlanguageQR(int id)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.languages.Where(x => x.ID == id).FirstOrDefault();
            string Cname = count.Lang_Name.ToString();
            Cname = Cname.Substring(0, 1);
            return Cname;
        }

        public static string getCount(string iso, UnogViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.countries.Where(x => x.languages_ID == model.lang_ID && x.ISO == iso).FirstOrDefault();
            string Cname = count.Short_Name.ToString();
            return Cname;
        }

        public static string getCountwithSrticle(string iso, UnogViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.countries.Where(x => x.languages_ID == model.lang_ID && x.ISO == iso).FirstOrDefault();
            string Cname = count.Short_Name.ToString();
            string Article = count.Article.ToString();
            if (Article == "l'")
            {
                return Article + Cname;
            }
            else
            {
                return Article + " " + Cname;
            }
           
        }

        //public static bool categoryname(UnogViewModel model)
        //{
        //    string str = model.tempname.ToString();
        //    //str = str.Substring(0, str.Length-1);
        //    gdgs1Entities db1 = new gdgs1Entities();
        //    var item = db1.final_temp.FirstOrDefault(p => p.Name == str);
        //    int x = item.categories_ID;
        //    var item1 = db1.categories.FirstOrDefault(p => p.ID == x);
        //    string cat = item1.Name.ToString();
        //    if (cat == "Country Report" || cat == "Country Report -Optional Procedure")
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        public static string Reportnumber(UnogViewModel model, string prep, int langg, string iso)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.languages.Where(p => p.ID == langg).FirstOrDefault();
            string lang = item.Lang_Name.ToString();

            string final = "";
           // string country = getCount(iso, model);
            string Country1 = getCountwithSrticle(iso, model);
            //bool cat = false;
            //cat = categoryname(model);
            //if (cat == true)
            //{
                switch (lang)
                {
                    case "English":
                        {
                            string cc = prep;
                            string[] com = cc.Split('-');

                            //case single number
                            if (com.Length == 1)
                            {
                                var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                                int x = Convert.ToInt32(prep);
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
                                        Fprep1 = "initial";
                                        final = " combined " + Fprep1 + " and " + Fprep2 + " reports " + Country1;
                                    }
                                    else { final = " combined " + Fprep1 + " and " + Fprep2 + " reports " + Country1; }
                                }
                                //case 1 - 5 or 3-10
                                if (x != y - 1)
                                {
                                    if (Fprep1 == "first")
                                    {
                                    Fprep1 = "initial";
                                    final = " combined " + Fprep1 + " to " + Fprep2 + " reports " + Country1;
                                }
                                else { final = " combined " + Fprep1 + " to " + Fprep2 + " reports " + Country1; }
                                }

                            }
                            break;
                        }
                case "French":
                    {
                        string cc = prep;
                        string[] com = cc.Split('-');
                        var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

                        //case single number
                        if (com.Length == 1)
                        {
                            int x = Convert.ToInt32(prep);
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
                            if (x == y-1)
                            {
                                if (Fprep1 == "premier")
                                {
                                    Fprep1 = "rapport initial";
                                    final = "rapport " + Country1 + " valant " + Fprep1 + " et " + Fprep2 + " rapports périodiques";
                                }
                                else { final = "rapport " + Country1 + " valant " + Fprep1 + " et " + Fprep2 + " rapports périodiques"; }

                            }
                            if (x != y - 1)
                            {
                                if (Fprep1 == "premier")
                                {
                                    Fprep1 = "rapport initial";
                                    final = "rapport " + Country1 + " valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
                                }
                                else { final = "rapport " + Country1 + " valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }

                            }
                            

                        }
                        break;
                    }
                case "Spanish":
                        {
                            final = SpanishNum(model, prep, iso);

                            break;
                        }
                }
            //}
            //if (cat == false)
            //{
            //    switch (lang)
            //    {
            //        case "English":
            //            {
            //                string cc = prep;
            //                string[] com = cc.Split('-');

            //                //case single number
            //                if (com.Length == 1)
            //                {
            //                    var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            //                    int x = Convert.ToInt32(prep);
            //                    final = x.ToOrdinalWords(engCultureInfo);
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
            //                string cc = prep;
            //                string[] com = cc.Split('-');
            //                var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

            //                //case single number
            //                if (com.Length == 1)
            //                {
            //                    int x = Convert.ToInt32(prep);
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
            //                final = SpanishNum1(model, prep, iso);

            //                break;
            //            }
            //    }

            //}
            return final;
        }
        public static string Reportnumbers(UnogViewModel model, string prep, int langg, string iso)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.languages.Where(p => p.ID == langg).FirstOrDefault();
            string lang = item.Lang_Name.ToString();

            string final = "";
            // string country = getCount(iso, model);
            string Country1 = getCountwithSrticle(iso, model);
            //bool cat = false;
            //cat = categoryname(model);
            //if (cat == true)
            //{
            switch (lang)
            {
                case "English":
                    {
                        string cc = prep;
                        string[] com = cc.Split('-');

                        //case single number
                        if (com.Length == 1)
                        {
                            var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                            int x = Convert.ToInt32(prep);
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
                                    Fprep1 = "initial";
                                    // final = Fprep1 + "and " + Fprep2 + " combined reports " + Country1;
                                    final = " combined " + Fprep1 + " and " + Fprep2 + " reports " + Country1;
                                }
                                else { final = " combined " + Fprep1 + " and " + Fprep2 + " reports " + Country1; }
                            }
                            //case 1 - 5 or 3-10
                            if (x != y - 1)
                            {
                                if (Fprep1 == "first")
                                {
                                    Fprep1 = "initial";
                                    //final = Fprep1 + " to " + Fprep2 + " combined reports " + Country1;
                                    final = " combined " + Fprep1 + " to " + Fprep2 + " reports " + Country1;
                                }
                                else { final = " combined " + Fprep1 + " to " + Fprep2 + " reports " + Country1; }
                            }

                        }
                        break;
                    }
                case "French":
                    {
                        string cc = prep;
                        string[] com = cc.Split('-');
                        var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

                        //case single number
                        if (com.Length == 1)
                        {
                            int x = Convert.ToInt32(prep);
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
                            if (x == y - 1)
                            {
                                if (Fprep1 == "premier")
                                {
                                    Fprep1 = "rapport initial";
                                    final = "rapport " + Country1 + " valant " + Fprep1 + " et " + Fprep2 + " rapports périodiques";
                                }
                                else { final = "rapport " + Country1 + " valant " + Fprep1 + " et " + Fprep2 + " rapports périodiques"; }

                            }
                            if (x != y - 1)
                            {
                                if (Fprep1 == "premier")
                                {
                                    Fprep1 = "rapport initial";
                                    final = "rapport " + Country1 + " valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
                                }
                                else { final = "rapport " + Country1 + " valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }

                            }


                        }
                        break;
                    }
                case "Spanish":
                    {
                        final = SpanishNums(model, prep, iso);

                        break;
                    }
            }
            //}
            //if (cat == false)
            //{
            //    switch (lang)
            //    {
            //        case "English":
            //            {
            //                string cc = prep;
            //                string[] com = cc.Split('-');

            //                //case single number
            //                if (com.Length == 1)
            //                {
            //                    var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            //                    int x = Convert.ToInt32(prep);
            //                    final = x.ToOrdinalWords(engCultureInfo);
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
            //                string cc = prep;
            //                string[] com = cc.Split('-');
            //                var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

            //                //case single number
            //                if (com.Length == 1)
            //                {
            //                    int x = Convert.ToInt32(prep);
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
            //                final = SpanishNum1(model, prep, iso);

            //                break;
            //            }
            //    }

            //}
            return final;
        }
        //public static string SpanishNum1(UnogViewModel model, string prep, string iso)
        //{
        //    string num1 = prep;
        //    string[] str = num1.Split('-');
        //    string final = "";
        //    var spanishCultureInfo = CultureInfo.CreateSpecificCulture("es-ES");
        //    if (str.Length == 1)
        //    {


        //        int num = Convert.ToInt32(prep);

        //        if (num <= 10)
        //        {
        //            final = num.ToOrdinalWords(spanishCultureInfo);
        //            final = final.Substring(0, 1).ToUpper() + final.Substring(1);
        //            if (final == "Primer")
        //            {
        //                final = "Informe iniciales";
        //            }
        //            else
        //            {
        //                final = num.ToOrdinalWords(spanishCultureInfo) + " informe periódico ";
        //            }
        //        }
        //        else
        //        {

        //            string chr = num1.Last().ToString();
        //            if (chr == "1" || chr == "3")
        //            {
        //                final = num1 + "\u1D49" + "\u02B3" + " informe periódico";
        //                final = final.Substring(0, 1).ToUpper() + final.Substring(1);
        //            }
        //            else
        //            {
        //                final = num1 + "\u1D52" + " informe periódico";
        //                final = final.Substring(0, 1).ToUpper() + final.Substring(1);
        //            }

        //        }
        //    }
        //    if (str.Length > 1)
        //    {
        //        int x = Convert.ToInt32(str[0]);
        //        int y = Convert.ToInt32(str[1]);

        //        if (x <= 10 & y <= 10)
        //        {
        //            string str1 = x.ToOrdinalWords(spanishCultureInfo);
        //            string str2 = y.ToOrdinalWords(spanishCultureInfo);
        //            if (x == y - 1)
        //            {
        //                final = "Informes periódicos " + str1 + " y " + str2 + " combinados ";
        //                if (str1 == "primer")
        //                {
        //                    final = "Informes periódicos inicial y " + str2 + " combinados ";
        //                }

        //            }
        //            else
        //            {
        //                final = "Informes periódicos " + str1 + " a " + str2 + " combinados ";
        //                if (str1 == "primer")
        //                {
        //                    final = "Informes periódicos inicial a " + str2 + " combinados ";
        //                }
        //            }

        //        }
        //        if (x <= 10 & y > 10)
        //        {
        //            string chr = str[0].Last().ToString();
        //            string final1 = "";
        //            string final2 = "";
        //            final1 = str[0] + "\u1D52";
        //            final2 = str[1] + "\u1D52";

        //            if (x == y - 1)
        //            {
        //                final = "Informes periódicos " + final1 + " y " + final2 + " combinados ";
        //            }
        //            else
        //            {
        //                final = "Informes periódicos " + final1 + " a " + final2 + " combinados ";
        //            }
        //        }
        //        if (x > 10 & y > 10)
        //        {
        //            string chr = str[0].Last().ToString();
        //            string final1 = "";
        //            string final2 = "";
        //            final1 = str[0] + "\u1D52";
        //            final2 = str[1] + "\u1D52";

        //            if (x == y - 1)
        //            {
        //                final = "a los informes periódicos " + final1 + " y " + final2 + " combinados ";
        //            }
        //            else
        //            {
        //                final = "a los informes periódicos " + final1 + " a " + final2 + " combinados ";
        //            }
        //        }
        //    }
        //    return final;

        //}

        public static string SpanishNum1(UnogViewModel model, string prep, string iso)
        {
            string num1 = prep;
            string[] str = num1.Split('-');
            string final = "";
            var spanishCultureInfo = CultureInfo.CreateSpecificCulture("es-ES");
            if (str.Length == 1)
            {


                int num = Convert.ToInt32(prep);

                //if (num <= 10)
                //{

                final = Toordinalspanish(prep);
                final = final.Substring(0, 1).ToUpper() + final.Substring(1);
                if (final == "Primero")
                {
                    final = "Informe inicial ";
                }
                else if (final == "Tercero")
                {
                    final = final.Remove(final.Length - 1);
                    final = final.Substring(0, 1).ToUpper() + final.Substring(1);
                    final = final + " informe periódico ";
                }
                else
                {
                    final = Toordinalspanish(prep);
                    final = final.Substring(0, 1).ToUpper() + final.Substring(1);
                    final = final + " informe periódico ";
                }
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
                        final = "Informes periódicos " + str1 + " y " + str2  + " combinados ";
                        if (str1 == "primero")
                        {
                            final = "Informes periódicos inicial y " + str2  + " combinados ";
                        }

                    }
                    else
                    {
                        final = "Informes periódicos " + str1 + " a " + str2 + " combinados ";
                        if (str1 == "primero")
                        {
                            final = "Informes periódicos inicial a " + str2 + " combinados ";
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

        public static string SpanishNum(UnogViewModel model, string prep, string iso)
        {
            string num1 = prep;
            string[] str = num1.Split('-');
            string final = "";
            string Country1 = getCountwithSrticle(iso, model);
            var spanishCultureInfo = CultureInfo.CreateSpecificCulture("es-es");

            //just one number no x-y
            if (str.Length == 1)
            {


                int num = Convert.ToInt32(prep);

                if (num <= 10)
                {
                    // final = num.ToOrdinalWords(spanishCultureInfo);
                    final = Toordinalspanish(num.ToString());
                    if (final == "primero")
                    {
                        final = "al informe inicial " + Country1;
                    }
                    else if (final == "tercero")
                    {
                        final = final.Remove(final.Length - 1);
                        final = "al " + final + " informe periódico " + Country1;
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
                   // string str1 = x.ToOrdinalWords(spanishCultureInfo);
                  //  string str2 = y.ToOrdinalWords(spanishCultureInfo);
                    string str1 = Toordinalspanish(x.ToString());
                    string str2 = Toordinalspanish(y.ToString());
                    if (x == y - 1)
                    {
                        final = "a los informes periódicos " + str1 + " y " + str2 + " combinados " + Country1;
                        if (str1 == "primero")
                        {
                            final = "a los informes periódicos primero y " + str2 + " combinados " + Country1;
                        }

                    }
                    else
                    {
                        final = "a los informes periódicos " + str1 + " a " + str2 + " combinados " + Country1;
                        if (str1 == "primero")
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
        public static string SpanishNums(UnogViewModel model, string prep, string iso)
        {
            string num1 = prep;
            string[] str = num1.Split('-');
            string final = "";
            string Country1 = getCountwithSrticle(iso, model);
            var spanishCultureInfo = CultureInfo.CreateSpecificCulture("es-es");

            //just one number no x-y
            if (str.Length == 1)
            {


                int num = Convert.ToInt32(prep);

                if (num <= 10)
                {
                    // final = num.ToOrdinalWords(spanishCultureInfo);
                    final = Toordinalspanish(num.ToString());
                    if (final == "primero")
                    {
                        final = "informe inicial " + Country1;
                    }
                    else if (final == "tercero")
                    {
                        final = final.Remove(final.Length - 1);
                        final = final + " informe periódico " + Country1;
                    }
                    else
                    {
                        final =final + " informe periódico " + Country1;
                    }
                }
                else
                {

                    string chr = num1.Last().ToString();
                    if (chr == "1" || chr == "3")
                    {
                        final =num1 + "\u1D49" + "\u02B3" + " informe periódico " + Country1;
                    }
                    else
                    {
                        final =num1 + "\u1D52" + " informe periódico " + Country1;
                    }

                }
            }
            if (str.Length > 1)
            {
                int x = Convert.ToInt32(str[0]);
                int y = Convert.ToInt32(str[1]);

                if (x <= 10 & y <= 10)
                {
                    // string str1 = x.ToOrdinalWords(spanishCultureInfo);
                    //  string str2 = y.ToOrdinalWords(spanishCultureInfo);
                    string str1 = Toordinalspanish(x.ToString());
                    string str2 = Toordinalspanish(y.ToString());
                    if (x == y - 1)
                    {
                        final = "informes periódicos " + str1 + " y " + str2 + " combinados " + Country1;
                        if (str1 == "primero")
                        {
                            final = "informes periódicos primero y " + str2 + " combinados " + Country1;
                        }

                    }
                    else
                    {
                        final = "informes periódicos " + str1 + " a " + str2 + " combinados " + Country1;
                        if (str1 == "primero")
                        {
                            final = "informes periódicos primero a " + str2 + " combinados " + Country1;
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
                        final = "informes periódicos " + final1 + " y " + final2 + " combinados " + Country1;
                    }
                    else
                    {
                        final = "informes periódicos " + final1 + " a " + final2 + " combinados " + Country1;
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
                        final = "informes periódicos " + final1 + " y " + final2 + " combinados " + Country1;
                    }
                    else
                    {
                        final = "informes periódicos " + final1 + " a " + final2 + " combinados " + Country1;
                    }
                }

            }
            return final;

        }
        public static string SpanishNumW(UnogViewModel model, string prep, string iso)
        {
            string num1 = prep;
            string[] str = num1.Split('-');
            string final = "";
            string Country1 = getCountwithSrticle(iso, model);
            var spanishCultureInfo = CultureInfo.CreateSpecificCulture("es-es");
            if (str.Length == 1)
            {


                int num = Convert.ToInt32(prep);

                if (num <= 10)
                {

                    //final = num.ToOrdinalWords(spanishCultureInfo);
                    final = Toordinalspanish(num.ToString());
                    if (final == "primero")
                    {
                        final = "al informe inicial ";
                    }
                    else if (final == "tercero")
                    {
                        final = final.Remove(final.Length - 1);
                        final = "al " + final + " informe periódico ";
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
                    //string str1 = x.ToOrdinalWords(spanishCultureInfo);
                  //  string str2 = y.ToOrdinalWords(spanishCultureInfo);

                    string str1 = Toordinalspanish(x.ToString());
                    string str2 = Toordinalspanish(y.ToString());
                    if (x == y - 1)
                    {
                        final = "a los informes periódicos " + str1 + " y " + str2 + " combinados";
                        if (str1 == "primero")
                        {
                            final = "a los informes periódicos inicial y " + str2 + " combinados";
                        }

                    }
                    else
                    {
                        final = "a los informes periódicos " + str1 + " a " + str2 + " combinados";
                        if (str1 == "primero")
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
        public static string SpanishNumWithoutArticle(UnogViewModel model, string prep, string iso)
        {
            string num1 = prep;
            string[] str = num1.Split('-');
            string final = "";
            string Country1 = getCountwithSrticle(iso, model);
            var spanishCultureInfo = CultureInfo.CreateSpecificCulture("es-es");
            if (str.Length == 1)
            {


                int num = Convert.ToInt32(prep);

                if (num <= 10)
                {

                    //final = num.ToOrdinalWords(spanishCultureInfo);
                    final = Toordinalspanish(num.ToString());
                    if (final == "primero")
                    {
                        final = "informe inicial";
                    }
                    else if (final == "tercero")
                    {
                        final = final.Remove(final.Length - 1);
                        final = final + " informe periódico";
                    }
                    else
                    {
                        final = final + " informe periódico";
                    }
                }
                else
                {

                    string chr = num1.Last().ToString();
                    if (chr == "1" || chr == "3")
                    {
                        final =num1 + "\u1D49" + "\u02B3" + " informe periódico";
                    }
                    else
                    {
                        final =num1 + "\u1D52" + " informe periódico";
                    }

                }
            }
            if (str.Length > 1)
            {
                int x = Convert.ToInt32(str[0]);
                int y = Convert.ToInt32(str[1]);

                if (x <= 10 & y <= 10)
                {
                    //string str1 = x.ToOrdinalWords(spanishCultureInfo);
                    //  string str2 = y.ToOrdinalWords(spanishCultureInfo);

                    string str1 = Toordinalspanish(x.ToString());
                    string str2 = Toordinalspanish(y.ToString());
                    if (x == y - 1)
                    {
                        final = "informes periódicos " + str1 + " y " + str2 + " combinados";
                        if (str1 == "primero")
                        {
                            final = "informes periódicos inicial y " + str2 + " combinados";
                        }

                    }
                    else
                    {
                        final = "informes periódicos " + str1 + " a " + str2 + " combinados";
                        if (str1 == "primero")
                        {
                            final = "informes periódicos inicial a " + str2 + " combinados";
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
                        final = "informes periódicos " + final1 + " y " + final2 + " combinados";
                    }
                    else
                    {
                        final = "informes periódicos " + final1 + " a " + final2 + " combinados";
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
                        final = "informes periódicos " + final1 + " y " + final2 + " combinados";
                    }
                    else
                    {
                        final = "informes periódicos " + final1 + " a " + final2 + " combinados";
                    }
                }

            }
            return final;

        }
        public static string ReportnumberWC(UnogViewModel model, string prep, int langg, string iso)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.languages.Where(p => p.ID == langg).FirstOrDefault();
            string lang = item.Lang_Name.ToString();

            string final = "";
            
            switch (lang)
            {
                case "English":
                    {
                        string cc = prep;
                        string[] com = cc.Split('-');

                        //case single number
                        if (com.Length == 1)
                        {
                            var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                            int x = Convert.ToInt32(prep);
                            final = x.ToOrdinalWords(engCultureInfo);
                            final = final.Substring(0, 1).ToUpperInvariant() + final.Substring(1);
                            if (final == "First")
                            {
                                final = "Initial report";
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
                                    Fprep1 = "Initial";
                                    //   final = Fprep1 + " and " + Fprep2 + " combined reports ";
                                    final = "Combined " + Fprep1 + " and " + Fprep2 + " reports ";
                                }
                                else { final = "Combined " + Fprep1 + " and " + Fprep2 + " reports "; }
                            }
                            //case 1 - 5 or 3-10
                            if (x != y - 1)
                            {
                                if (Fprep1 == "first")
                                {
                                    Fprep1 = "Initial";
                                    final = "Combined " + Fprep1 + " to " + Fprep2 + " reports ";
                                }
                                else { final = "Combined " + Fprep1 + " to " + Fprep2 + " reports "; }
                            }

                        }
                        break;
                    }
                case "French":
                    {
                        string cc = prep;
                        string[] com = cc.Split('-');
                        var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

                        //case single number
                        if (com.Length == 1)
                        {
                            int x = Convert.ToInt32(prep);
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

                            if (x == y - 1)
                            {
                                if (Fprep1 == "premier")
                                {
                                    Fprep1 = "initial ";
                                    final = "Rapport valant " + Fprep1 + " et " + Fprep2 + " rapports périodiques";
                                }
                                else { final = "Rapport valant " + Fprep1 + " et " + Fprep2 + " rapports périodiques"; }
                            }
                            if (x != y - 1)
                            {
                                if (Fprep1 == "premier")
                                {
                                    Fprep1 = "initial ";
                                    final = "Rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
                                }
                                else { final = "Rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }
                            }

                        }
                        break;
                    }
                case "Spanish":
                    {
                        final = SpanishNum1(model, prep, iso);

                        break;
                    }
                default:
                    {
                        final = "Not Supported";
                        break;
                    }
            }

            //}
            return final;
        }
        public static string ReportnumberW(UnogViewModel model, string prep, int langg, string iso)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.languages.Where(p => p.ID == langg).FirstOrDefault();
            string lang = item.Lang_Name.ToString();

            string final = "";
            //string country = getCount(iso, model);
            // string Country1 = getCountwithSrticle(iso, model);
            //  bool cat = false;
            //  cat = categoryname(model);
            //if (cat == true)
            //{
            switch (lang)
            {
                case "English":
                    {
                        string cc = prep;
                        string[] com = cc.Split('-');

                        //case single number
                        if (com.Length == 1)
                        {
                            var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                            int x = Convert.ToInt32(prep);
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
                                    Fprep1 = "initial";
                                     final = " combined " + Fprep1 + " and " + Fprep2 + " reports";
                                    
                                }
                                else { final = " combined " + Fprep1 + " and " + Fprep2 + " reports"; }
                            }
                            //case 1 - 5 or 3-10
                            if (x != y - 1)
                            {
                                if (Fprep1 == "first")
                                {
                                    Fprep1 = "initial";
                                    final = " combined " + Fprep1 + " to " + Fprep2 + " reports";
                                }
                                else { final = " combined " + Fprep1 + " to " + Fprep2 + " reports"; }
                            }

                        }
                        break;
                    }
                case "French":
                    {
                        string cc = prep;
                        string[] com = cc.Split('-');
                        var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

                        //case single number
                        if (com.Length == 1)
                        {
                            int x = Convert.ToInt32(prep);
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
                            if (x == y - 1)
                            {
                                if (Fprep1 == "premier")
                                {
                                    Fprep1 = "rapport initial";
                                    final = "rapport valant " + Fprep1 + " et " + Fprep2 + " rapports périodiques";
                                }
                                else { final = "rapport valant " + Fprep1 + " et " + Fprep2 + " rapports périodiques"; }


                            }
                            if (x != y - 1)
                            {
                                if (Fprep1 == "premier")
                                {
                                    Fprep1 = "rapport initial";
                                    final = "rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
                                }
                                else { final = "rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }


                            }

                        }
                        break;
                    }
                case "Spanish":
                    {
                        final = SpanishNumW(model, prep, iso);

                        break;
                    }
                default:
                    {
                        final = "Not Supported";
                        break;
                    }
            }
           
            return final;
        }
        public static string ReportnumberWS(UnogViewModel model, string prep, int langg, string iso)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var item = db1.languages.Where(p => p.ID == langg).FirstOrDefault();
            string lang = item.Lang_Name.ToString();

            string final = "";
            //string country = getCount(iso, model);
            // string Country1 = getCountwithSrticle(iso, model);
            //  bool cat = false;
            //  cat = categoryname(model);
            //if (cat == true)
            //{
            switch (lang)
            {
                case "English":
                    {
                        string cc = prep;
                        string[] com = cc.Split('-');

                        //case single number
                        if (com.Length == 1)
                        {
                            var engCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                            int x = Convert.ToInt32(prep);
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
                                    Fprep1 = "initial";
                                    final = "Combined " + Fprep1 + " and " + Fprep2 + " reports ";
                                }
                                else { final = "Combined " + Fprep1 + " and " + Fprep2 + " reports "; }
                            }
                            //case 1 - 5 or 3-10
                            if (x != y - 1)
                            {
                                if (Fprep1 == "first")
                                {
                                    Fprep1 = "initial";
                                    final = "Combined " + Fprep1 + " to " + Fprep2 + " reports ";
                                }
                                else { final = "Combined " + Fprep1 + " to " + Fprep2 + " reports "; }
                            }

                        }
                        break;
                    }
                case "French":
                    {
                        string cc = prep;
                        string[] com = cc.Split('-');
                        var frenchCultureInfo = CultureInfo.CreateSpecificCulture("fr-fr");

                        //case single number
                        if (com.Length == 1)
                        {
                            int x = Convert.ToInt32(prep);
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
                            if (x == y - 1)
                            {
                                if (Fprep1 == "premier")
                                {
                                    Fprep1 = "rapport initial";
                                    final = "rapport valant " + Fprep1 + " et " + Fprep2 + " rapports périodiques";
                                }
                                else { final = "rapport valant " + Fprep1 + " et " + Fprep2 + " rapports périodiques"; }


                            }
                            if (x != y - 1)
                            {
                                if (Fprep1 == "premier")
                                {
                                    Fprep1 = "rapport initial";
                                    final = "rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques";
                                }
                                else { final = "rapport valant " + Fprep1 + " à " + Fprep2 + " rapports périodiques"; }


                            }

                        }
                        break;
                    }
                case "Spanish":
                    {
                        final = SpanishNumWithoutArticle(model, prep, iso);

                        break;
                    }
                default:
                    {
                        final = "Not Supported";
                        break;
                    }
            }

            return final;
        }

        public static string getVerisons(UnogViewModel model)
        {
            string[] s;
            if (model.ver == null)
            {
                string ss = Tlanguage(model.lang_ID);
                s = ss.Split(',');
            }
            else
            {
                s = model.ver.Split(',');
                string a = Tlanguage(model.lang_ID);
                Array.Resize(ref s, s.Length + 1);
                s[s.Length - 1] = a;
            }
            
            
            // s = SortVirsion(s);

            s = s.OrderBy(q => q).ToArray();
            for (int i = 0; i < 1; i++)
            {
               s[i]=CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s[i]);
               
            }
           
            string lang = Olanguage(model.lang_ID);
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
            //Fvirs = char.ToUpper(Fvirs[0]) + Fvirs.Substring(1);
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
        public static string[] SortVirsion(string[] list)
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
    }
}