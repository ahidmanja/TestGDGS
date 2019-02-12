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
using System.Xml.Linq;
using System.Xml;
using DocumentFormat.OpenXml.CustomProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace IdentitySample.Controllers
{
    public class JIUController : Controller
    {
        static Assembly g_assembly;
        static DocX g_document;
        private gdgs1Entities db = new gdgs1Entities();
        // GET: JIU
        public ActionResult Index()
        {
            return View();
        }
      

        //public static void Changegdoc(string fileName)
        //{
        //       using (WordprocessingDocument
        //        document = WordprocessingDocument.Open(fileName, true))

        //    {

        //        DocumentSettingsPart settingsPart =


        //        document.MainDocumentPart.GetPartsOfType<DocumentSettingsPart>().First();



        //        // Create object to update fields on open


        //        UpdateFieldsOnOpen updateFields = new UpdateFieldsOnOpen();

        //        updateFields.Val = new DocumentFormat.OpenXml.OnOffValue(true);



        //        // Insert object into settings part.


        //        settingsPart.Settings.PrependChild<UpdateFieldsOnOpen>(updateFields);

        //        settingsPart.Settings.Save();



        //    }
        //}

        [HttpPost]
        public ActionResult FindTemplate(JIUViewModel model)
        {
            gdgs1Entities db1 = new gdgs1Entities();
            JIUViewModel Newmodel = new JIUViewModel();
            //Filtering possible Matches according to Symbole structure
            string sym = model.Sym.ToString();
            int Count = sym.Split('/').Count();
            int i = 0;
            int index = 0;

            int numofmatches = 0;
            //string Matches ="";


            List<JIUViewModel> List = new List<JIUViewModel>();

            List = db1.final_temp.Where(p => p.Count == Count).Select(Item => new JIUViewModel
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
    
        public ActionResult Generate(JIUViewModel model)
        {
            //get temp name from reg expression plus lang plus symb
            string fname = model.tempname.ToString();
            g_assembly = Assembly.GetExecutingAssembly();

       //     string source = Server.MapPath(Path.Combine("/", "IN/" + fname + ".docx"));
         //  string Dest = Server.MapPath(Path.Combine("/", "OUT/" + fname + ".docx"));

             string source = Server.MapPath(Path.Combine("/", "GDGS/IN/" + fname + ".docx"));
             string Dest = Server.MapPath(Path.Combine("/", "GDGS/OUT/" + fname + ".docx"));

            string tempname = fname.Remove(fname.Length - 1);
            model.tempname = tempname;
            g_document = DocX.Load(source);
            g_document = CreateDocA(DocX.Load(source), model,source);
            
            g_document.SaveAs(Dest);
            g_document.AddCoreProperty("dc:title", model.Sym.ToString());
           // g_document.AddCoreProperty("dc:author", model.Sym.ToString());
            g_document.Save();
         //   Changegdoc(source);
            return RedirectToAction("download", "JIU", new { name = fname });

        }
       
        public ActionResult download(string name)
        {
            return File(Url.Content("/GDGS/OUT/" + name + ".docx"), "text/plain", name + ".docx");

           // return File(Url.Content("/OUT/" + name + ".docx"), "text/plain", name + ".docx");
        }
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
        public static string getlanguageQR(int id)
        {
            gdgs1Entities db1 = new gdgs1Entities();

            var count = db1.languages.Where(x => x.ID == id).FirstOrDefault();
            string Cname = count.Lang_Name.ToString();
            Cname = Cname.Substring(0, 1);
            return Cname;
        }
        public static string CleanInvalidXmlChars(string StrInput)
        {
            //Returns same value if the value is empty.
            if (string.IsNullOrWhiteSpace(StrInput))
            {
                return StrInput;
            }
            // From xml spec valid chars:
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]    
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF.
            string RegularExp = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
            return Regex.Replace(StrInput, RegularExp, String.Empty);
        }
        public static DocX CreateDocA(DocX template, JIUViewModel model , string source)
        {
            string sym = model.Sym.ToString();
            string[] SymStr = sym.Split('/');
            string[] sym1 = SymStr.Skip(1).ToArray();
            string Fsym = String.Join("/", sym1);
            int yearppos = 0;
            

            //document Language "Section"
            string Ftlang = "";
            Ftlang = Tlanguage(model.lang_ID);
            //ORiginal Language
            string Folang = "";
           // Folang = Olanguage1(model.Olang_ID);


            string FPname = "[Prepared By]";
            //Typist
            if (!String.IsNullOrEmpty(model.Pname))
            {
                //string[] arr = model.Pname.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                //FPname = "";
                //for (int x = 0; x < arr.Length-1; x++)
                //{

                //    FPname = FPname+ arr[x] +"\n";

                //}
                //int xs = arr.Length;
                //FPname = FPname + arr[xs-1];
                FPname = model.Pname.ToString();
                    
            }
            //Novacode.Formatting prepared = new Novacode.Formatting();
            //prepared.Italic = true;
            //prepared.Bold = true;
            //prepared.Size=14;


           // template.ReplaceText("Pname", FPname,false,RegexOptions.None,prepared);

            //Agenda Title
            string FJtitle = "[Title]";
            if (!String.IsNullOrEmpty(model.JTitle))
            {
                FJtitle = model.JTitle.ToString();
            }
          ///
            //Year Jdate

            string structure = model.structure.ToString();
            string[] str = structure.Split('/');
            string FJdate = "[YEAR]";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == "&")
                {
                    yearppos = i;
                    FJdate = SymStr[i];
                }
               
            }
           
            //create barcode
            string Fbar = "";
            if (!String.IsNullOrEmpty(model.Gdoc))
            {
                Fbar = model.Gdoc.ToString();
                Fbar = "*" + Fbar + "*";
            }

            string gdoc = "";
            string Fgdocf="";
            if (!String.IsNullOrEmpty(model.Gdoc))
            {
                gdoc = model.Gdoc.ToString();
                gdoc = gdoc.Insert(2, "-");
                Fgdocf = gdoc;
            }
            
           
            
            DateTime xxx = DateTime.Now;

        

            template.AddCustomProperty(new CustomProperty("sym1", Fsym));
          
            template.AddCustomProperty(new CustomProperty("symh", sym));
            //template.AddCustomProperty(new CustomProperty("symh1", sym));
            //template.AddCustomProperty(new CustomProperty("symh2", sym));

            // template.ReplaceText(sym, sym, false, RegexOptions.IgnoreCase);



            template.AddCustomProperty(new CustomProperty("jdate", FJdate));
            //   template.ReplaceText("sdate", Fsdate, false, RegexOptions.IgnoreCase);


            template.AddCustomProperty(new CustomProperty("olang", Ftlang));
            template.AddCustomProperty(new CustomProperty("gdoc", gdoc));
            template.AddCustomProperty(new CustomProperty("gdoc1", gdoc));
            // template.ReplaceText("gdoc", gdoc, false, RegexOptions.IgnoreCase);

            template.AddCustomProperty(new CustomProperty("gdocf", Fgdocf));
            template.AddCustomProperty(new CustomProperty("test", Fgdocf));
            //template.AddCustomProperty(new CustomProperty("gdocf1", Fgdocf));
            //template.AddCustomProperty(new CustomProperty("gdocf2", Fgdocf));
            // template.ReplaceText("gdocf", Fgdocf, false, RegexOptions.IgnoreCase);

            template.AddCustomProperty(new CustomProperty("tlang", ""));
            // template.ReplaceText("tlang", Ftlang, false, RegexOptions.IgnoreCase);

            template.AddCustomProperty(new CustomProperty("jtitle", FJtitle));
            // template.ReplaceText("atitle", Fatitle, false, RegexOptions.IgnoreCase);

            template.AddCustomProperty(new CustomProperty("Pname", FPname));
            // template.ReplaceText("stitle", Fstitle, false, RegexOptions.IgnoreCase);

         

            template.AddCustomProperty(new CustomProperty("bar", Fbar));
            //   template.ReplaceText("bar", Fbar, false, RegexOptions.IgnoreCase);

            template.AddCustomProperty(new CustomProperty("Date-Generated", xxx));
            template.AddCustomProperty(new CustomProperty("Org", "JIU"));
            template.AddCustomProperty(new CustomProperty("Entity", "JIU"));
            template.AddCustomProperty(new CustomProperty("doctype", "Main"));
            template.AddCustomProperty(new CustomProperty("category", "Report"));

            using (WordprocessingDocument
               
               document = WordprocessingDocument.Open(source, true))

            {

                DocumentSettingsPart settingsPart =


                document.MainDocumentPart.GetPartsOfType<DocumentSettingsPart>().First();



                // Create object to update fields on open


                UpdateFieldsOnOpen updateFields = new UpdateFieldsOnOpen();

                updateFields.Val = new DocumentFormat.OpenXml.OnOffValue(true);



                // Insert object into settings part.


                settingsPart.Settings.PrependChild<UpdateFieldsOnOpen>(updateFields);

                settingsPart.Settings.Save();



            }

            return template;
        }
        
    }
}