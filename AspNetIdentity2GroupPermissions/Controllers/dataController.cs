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
    public class dataController : Controller
    {
         
        private DataRepo repo = new DataRepo();
        private gdgs1Entities db = new gdgs1Entities();

        // GET: data
        public ActionResult Index()
        {
            string[] filesLoc = Directory.GetFiles(Server.MapPath("~/Meta/"));

            foreach (string file in filesLoc)
            {
             
               
                DataViewModel result = new DataViewModel();
                string fname = Path.GetFileName(file);
                if (file.Contains("-indexed-") == false)
                {
                    string source = Server.MapPath(Path.Combine("/", "Meta/" + fname));
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



        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(repo.Read().ToDataSourceResult(request));
        }
        public ActionResult ReadOHCHR()
        {

            string[] filesLoc = Directory.GetFiles(Server.MapPath("~/Meta/"));

            foreach (string file in filesLoc)
            {


                DataViewModel result = new DataViewModel();
                string fname = Path.GetFileName(file);
                if (file.Contains("-indexed-") == false)
                {
                    string source = Server.MapPath(Path.Combine("/", "Meta/" + fname));
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
        [HttpPost]
        public ActionResult ReadOHCHR([DataSourceRequest] DataSourceRequest request,string Category , string Entity)
        {

            var result = repo.Read();
            //var result = repo.ReadFilteredOHCHR(model);
            result = result.Where(a => a.Org == "OHCHR");
            if (!String.IsNullOrEmpty(Entity))
            {
                result = result.Where(a => a.Entity == Entity);
            }
            if (!String.IsNullOrEmpty(Category))
            {
                result = result.Where(a => a.Category == Category);
            }
            //if (model.DocType != null)
            //{
            //    result = result.Where(a => a.DocType == model.DocType).ToList();
            //}
            //if (model.Olang != null)
            //{
            //    result = result.Where(a => a.Olang == model.Olang).ToList();
            //}
            //if (model.Dist != null)
            //{
            //    result = result.Where(a => a.Dist == model.Dist).ToList();
            //}
            //if (model.Date != null)
            //{
            //    result = result.Where(a => a.Date == model.Date).ToList();
            //}
            //if (model.Snum != null)
            //{
            //    result = result.Where(a => a.Snum == model.Snum).ToList();
            //}
            //if (model.Sdate != null)
            //{
            //    result = result.Where(a => a.Sdate == model.Sdate).ToList();
            //}
            //if (model.Sdate != null)
            //{
            //    result = result.Where(a => a.Sdate == model.Sdate).ToList();
            //}
            //if (model.Atitle != null)
            //{
            //    result = result.Where(a => a.Atitle == model.Atitle).ToList();
            //}
            //if (model.Anum != null)
            //{
            //    result = result.Where(a => a.Anum == model.Anum).ToList();
            //}

            return Json(result.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadSR()
        {

            string[] filesLoc = Directory.GetFiles(Server.MapPath("~/Meta/"));

            foreach (string file in filesLoc)
            {


                DataViewModel result = new DataViewModel();
                string fname = Path.GetFileName(file);
                if (file.Contains("-indexed-") == false)
                {
                    string source = Server.MapPath(Path.Combine("/", "Meta/" + fname));
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

        [HttpPost]
        public ActionResult ReadSR([DataSourceRequest] DataSourceRequest request)
        {
            var result = repo.Read();
            result = result.Where(a => a.Org == "SR").ToList();
            return Json(repo.Read().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadECE()
        {

            string[] filesLoc = Directory.GetFiles(Server.MapPath("~/Meta/"));

            foreach (string file in filesLoc)
            {


                DataViewModel result = new DataViewModel();
                string fname = Path.GetFileName(file);
                if (file.Contains("-indexed-") == false)
                {
                    string source = Server.MapPath(Path.Combine("/", "Meta/" + fname));
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
        [HttpPost]
        public ActionResult ReadECE([DataSourceRequest] DataSourceRequest request)
        {
            var result = repo.Read();
            result = result.Where(a => a.Org == "ECE").ToList();
            return Json(repo.Read().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, DataViewModel item)
        {
            if (item != null && ModelState.IsValid)
            {
                repo.Create(item);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }


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
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, DataViewModel item)
        {
            if (item != null && ModelState.IsValid)
            {
                repo.Update(item);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, DataViewModel item)
        {
            if (item != null)
            {
                repo.Destroy(item);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }
    }
}