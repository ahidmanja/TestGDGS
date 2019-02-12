using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Novacode;
using System.Reflection;
using System.IO;
using System.Globalization;
using Humanizer;
using DocumentFormat.OpenXml.CustomProperties;
using DocumentFormat.OpenXml.Packaging;

namespace IdentitySample.Controllers
{
    public class OIndexController : Controller
    {

        private DataRepo repo = new DataRepo();
        private gdgs1Entities db = new gdgs1Entities();
        public static string WDGetCustomProperty(string fileName, string propertyName)
        {
            // Given a document name and a custom property, retrieve the value of the property.

            string returnValue = null;

            using (var document = WordprocessingDocument.Open(fileName, false))
            {
                var customProps = document.CustomFilePropertiesPart;
                if (customProps != null)
                {
                    // No custom properties? Nothing to return, in that case.
                    var props = customProps.Properties;
                    if (props != null)
                    {
                        // This will trigger an exception is the property's Name property is null, but
                        // if that happens, the property is damaged, and probably should raise an exception.
                        var prop = props.
                          Where(p => ((CustomDocumentProperty)p).Name.Value == propertyName).FirstOrDefault();
                        // Does the property exist? If so, get the return value.
                        if (prop != null)
                        {
                            returnValue = prop.InnerText;
                        }
                    }
                }
            }
            return returnValue;
        }
        // GET: OIndex
        public ActionResult Index()
        {
            string[] filesLoc = Directory.GetFiles(Server.MapPath("~/Meta/"));

            foreach (string file in filesLoc)
            {


                DataViewModel result = new DataViewModel();
                string fname = Path.GetFileName(file);
                if (file.Contains("-indexed-") == false)
                {
                    string source = Server.MapPath(Path.Combine("/GDGS/", "Meta/" + fname));
                    string symh = WDGetCustomProperty(source, "symh");
                    string tlang = WDGetCustomProperty(source, "tlang");
                    string olang = WDGetCustomProperty(source, "olang");
                    string sdate = WDGetCustomProperty(source, "sdate");
                    string anum = WDGetCustomProperty(source, "anum");
                    string atitle = WDGetCustomProperty(source, "atitle");
                    string count = WDGetCustomProperty(source, "countw");
                    string prep = WDGetCustomProperty(source, "prepwc");
                    string stitle = WDGetCustomProperty(source, "stitle");
                    string gdoc = WDGetCustomProperty(source, "gdoc");
                    string bar = WDGetCustomProperty(source, "bar");
                    string dist = WDGetCustomProperty(source, "dist");
                    string date = WDGetCustomProperty(source, "date");
                    string ldate = WDGetCustomProperty(source, "ldate");
                    string dname = WDGetCustomProperty(source, "dname");
                    string loca = WDGetCustomProperty(source, "loca");
                    string snum = WDGetCustomProperty(source, "snum");
                    string mnum = WDGetCustomProperty(source, "mnum");
                    string org = WDGetCustomProperty(source, "Org");
                    string entity = WDGetCustomProperty(source, "Entity");
                    string doctype = WDGetCustomProperty(source, "doctype");
                    string category = WDGetCustomProperty(source, "category");
                    string lname1 = WDGetCustomProperty(source, "lname1");
                    string lname2 = WDGetCustomProperty(source, "lname2");

                    string subcat = WDGetCustomProperty(source, "subcategory");
                    result.Org = org;
                    result.Entity = entity;
                    result.DocType = doctype;
                    result.Category = category;
                    result.Lname1 = lname1;
                    result.Lname2 = lname2;
                    result.Subcat = subcat;


                    result.Anum = anum;
                    result.Atitle = atitle;
                    result.Bar = bar;
                    result.Count = count;
                    result.Date = date;
                    result.Dist = dist;
                    result.Dname = dname;
                    result.Gdoc = gdoc;
                    result.Ldate = ldate;
                    result.Loca = loca;
                    result.Mnum = mnum;
                    result.Olang = olang;
                    result.Prep = prep;
                    result.Sdate = sdate;
                    result.Snum = snum;
                    result.Stitle = stitle;
                    result.Symh = symh;
                    result.Tlang = tlang;
                    result.FName = fname;
                    repo.Create(result);
                    System.IO.File.Move(source, source + "-indexed-.docx");
                }
                else
                { }

            }
            return View();
        }
        public ActionResult upload()
        {
            return View();

            
        }

        public ActionResult download(string name)
        {
            name = name + "-indexed-.docx";
            // return File(Url.Content("/GDGS/Meta/" + name + ".docx"), "text/plain", name + ".docx");
            // return File(Url.Content("/Meta/" + name ), "text/plain", name + ".docx");

            //byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\Ahid\AspNetIdentity2GroupPermissions-master - Copy\AspNetIdentity2GroupPermissions\Meta\"+ name);
            //string fileName = name;
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            return File(Url.Content("/GDGS/Meta/" + name), "application/docx", name);
        }
        public ActionResult ReadOHCHR([DataSourceRequest] DataSourceRequest request,string Org, string Category, string Entity ,string DocType
            , string Olang, string Dist, string Date, string Snum, string Sdate, string Atitle , string Anum , string Count , string Symh)
        {

            var result = repo.Read();
            //var result = repo.ReadFilteredOHCHR(model);
            if (!String.IsNullOrEmpty(Org))
            {
                result = result.Where(a => a.Org == Org);
            }
            if (!String.IsNullOrEmpty(Symh))
            {
                result = result.Where(a => a.Symh == Symh);
            }

            if (!String.IsNullOrEmpty(Entity))
            {
                result = result.Where(a => a.Entity == Entity);
            }
            if (!String.IsNullOrEmpty(Category))
            {
                result = result.Where(a => a.Category == Category);
            }
            if (!String.IsNullOrEmpty(DocType))
            {
                result = result.Where(a => a.DocType == DocType);
            }
            if (!String.IsNullOrEmpty(Olang))
            {
                result = result.Where(a => a.Olang == Olang).ToList();
            }
            if (!String.IsNullOrEmpty(Dist))
            {
                result = result.Where(a => a.Dist == Dist);
            }
            if (!String.IsNullOrEmpty(Date))
            {
                result = result.Where(a => a.Date == Date).ToList();
            }
            if (!String.IsNullOrEmpty(Snum))
            {
                result = result.Where(a => a.Snum == Snum);
            }
            if (!String.IsNullOrEmpty(Sdate))
            {
                result = result.Where(a => a.Sdate == Sdate);
            }

            if (!String.IsNullOrEmpty(Atitle))
            {
                result = result.Where(a => a.Atitle == Atitle);
            }
            if (!String.IsNullOrEmpty(Anum))
            {
                result = result.Where(a => a.Anum == Anum);
            }
            if (!String.IsNullOrEmpty(Count))
            {
                result = result.Where(a => a.Count == Count);
            }

            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetailTemplate_HierarchyBinding_Doc(int ID, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(repo.Read()
                .Where(o => o.ID == ID)
                .ToDataSourceResult(request));
        }
    }
}