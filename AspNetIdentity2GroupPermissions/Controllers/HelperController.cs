using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class HelperController : Controller
    {
        private gdgs1Entities db = new gdgs1Entities();

        public JsonResult ServerFiltering_GetDate(string text , string org)
        {


            var date = db.data.Select(result => new DataViewModel
            {

                Date = result.date,
                Org = result.org

            });
            date = date.Where(p => p.Org == org).Distinct();
            if (!string.IsNullOrEmpty(text))
            {
                date = date.Where(p => p.Date.Contains(text)).Distinct();
            }

            return Json(date, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_GetSym(string text, string org)
        {


            var sym = db.data.Select(result => new DataViewModel
            {

                Symh = result.symh,
                Org = result.org

            });
            sym = sym.Where(p => p.Org == org).Distinct();
            if (!string.IsNullOrEmpty(text))
            {
                sym = sym.Where(p => p.Symh.Contains(text)).Distinct();
            }

            return Json(sym, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_GetOrg(string text)
        {


            var final = db.data.Select(result => new DataViewModel
            {

               
                Org = result.org

            });
          
            if (!string.IsNullOrEmpty(text))
            {
                final = final.Where(p => p.Org.Contains(text)).Distinct();
            }

            return Json(final, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_GetAtitle(string text , string org)
        {


            var atitle = db.data.Select(result => new DataViewModel
            {

                Atitle = result.atitle,
                Org = result.org

            });
            atitle = atitle.Where(p => p.Org == org).Distinct();
            if (!string.IsNullOrEmpty(text))
            {
                atitle = atitle.Where(p => p.Atitle.Contains(text)).Distinct();
            }

            return Json(atitle, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_GetAnum(string text, string org)
        {


            var anum = db.data.Select(result => new DataViewModel
            {

                Anum = result.anum,
                Org = result.org

            });
            anum = anum.Where(p => p.Org == org).Distinct();
            if (!string.IsNullOrEmpty(text))
            {
                anum = anum.Where(p => p.Anum.Contains(text)).Distinct();
            }

            return Json(anum, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_GetSdate(string text, string org)
        {


            var sdate = db.data.Select(result => new DataViewModel
            {

                Sdate = result.sdate,
                Org = result.org

            });
            sdate = sdate.Where(p => p.Org == org).Distinct();
            if (!string.IsNullOrEmpty(text))
            {
                sdate = sdate.Where(p => p.Sdate.Contains(text)).Distinct();
            }

            return Json(sdate, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_GetComs(string text, string org)
        {


            var committees = db.data.Select(result => new DataViewModel
            {
              
                Entity = result.entity,
                Org=result.org
                
            });
            committees = committees.Where(p => p.Org == org).Distinct();
            if (!string.IsNullOrEmpty(text))
            {
                committees = committees.Where(p => p.Entity.Contains(text)).Distinct();
            }

            return Json(committees, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_GetdocT(string text, string org)
        {


            var doc = db.data.Select(result => new DataViewModel
            {

                DocType = result.doctype,
                Org = result.org

            });
            doc = doc.Where(p => p.Org == org).Distinct();
            if (!string.IsNullOrEmpty(text))
            {
                doc = doc.Where(p => p.DocType.Contains(text)).Distinct();
            }

            return Json(doc, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_Getcat(string text, string org)
        {


            var cat = db.data.Select(result => new DataViewModel
            {

                Category = result.category,
                Org = result.org

            });
            cat = cat.Where(p => p.Org == org).Distinct();
            if (!string.IsNullOrEmpty(text))
            {
                cat = cat.Where(p => p.Category.Contains(text)).Distinct();
            }

            return Json(cat, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_Getolang(string text, string org)
        {


            var olang = db.data.Select(result => new DataViewModel
            {

                Olang = result.olang,
                Org = result.org

            });
            olang = olang.Where(p => p.Org == org).Distinct();
            if (!string.IsNullOrEmpty(text))
            {
                olang = olang.Where(p => p.Olang.Contains(text)).Distinct();
            }

            return Json(olang, JsonRequestBehavior.AllowGet);
        }
        
             public JsonResult ServerFiltering_GetSnmu(string text, string org)
        {


            var snum = db.data.Select(result => new DataViewModel
            {

                Snum = result.snum,
                Org = result.org

            });
            snum = snum.Where(p => p.Org == org).Distinct();
            if (!string.IsNullOrEmpty(text))
            {
                snum = snum.Where(p => p.Snum.Contains(text)).Distinct();
            }

            return Json(snum, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_GetDist(string text, string org)
        {


            var dist = db.data.Select(result => new DataViewModel
            {

                Dist = result.dist,
                Org = result.org

            });
            dist = dist.Where(p => p.Org == org);
            if (!string.IsNullOrEmpty(text))
            {
                dist = dist.Where(p => p.Dist.Contains(text)).Distinct();
            }

            return Json(dist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ServerFiltering_GetCount(string text, string org)
        {


            var count = db.data.Select(result => new DataViewModel
            {

                Count = result.count,
                Org = result.org

            });
            count = count.Where(p => p.Org == org);
            if (!string.IsNullOrEmpty(text))
            {
                count = count.Where(p => p.Count.Contains(text)).Distinct();
            }

            return Json(count, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public string Toordinalspanish(string num)
        {
            string final = "";
            string[] Unidad = { "", "primero", "segundo", "tercero",
            "cuarto", "quinto", "sexto", "septimo", "octavo",
            "noveno" };

            string[] Decena = { "", "decimo", "vigesimo", "trigesimo",
            "cuadragesimo", "quincuagesimo", "sexagesimo",
            "septuagesimo", "octogesimo", "nonagesimo" };
            string[] Centena = {"", "centesimo", "ducentesimo",
            "tricentesimo", " cuadringentesimo", " quingentesimo",
            " sexcentesimo", " septingentesimo", " octingentesimo",
            " noningentesimo"};

            int N = Convert.ToInt32(num);
            int u = N % 10;
            int d = (N / 10) % 10;
            int c = N / 100;
            if (N >= 100)
            {
                final=Centena[c] + " " + Decena[d] + " " + Unidad[u];
            }
            else
            {
                if (N >= 10)
                {
                    final=Decena[d] + " " + Unidad[u];
                }
                else
                {
                    final=Unidad[N];
                }
            }
            return final;
        }
        public JsonResult GetUTypes(string categories)
        {

            var types = db.final_temp.AsQueryable();

            if (categories != null)
            {
                types = types.Where(p => p.Symbole == categories);
            }

            return Json(types.Select(p => new { tID = p.Name, tName = p.category.Name }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVersions1()
        {

            var Item = db.olanguages.AsQueryable();
            
           
         //  Item = Item.Where(p => p.languages_ID == 1);

            return Json(Item.Select(p => new { Text = p.Name, Value = p.Name }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVersions(int? t)
        {

            var Item = db.olanguages.AsQueryable();
           
            if (t != null)
            {
                Item = Item.Where(p => p.languages_ID ==t);
            }

            return Json(Item.Select(p => new { Text = p.Name, Value = p.Name }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCascadeCount(int? t, string Filter )
        {

            var Item = db.countries.AsQueryable();

            if (t != null)
            {
                Item = Item.Where(p => p.languages_ID == t);
            }
            if (!string.IsNullOrEmpty(Filter))
            {
                Item = Item.Where(p => p.Name.Contains(Filter));
            }

            return Json(Item.Select(p => new { ID = p.ID, Name = p.Name }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCascadedist(int? t)
        {
            
            var Item = db.distrbutions.AsQueryable();

            if (t != null)
            {
                Item = Item.Where(p => p.languages_ID == t);
            }

            return Json(Item.Select(p => new { ID = p.ID, Name = p.Name }), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetOlang(int? t)
        {

            var Item = db.olanguages.AsQueryable();

            if (t != null)
            {
                Item = Item.Where(p => p.languages_ID == t);
            }

            return Json(Item.Select(p => new { ID = p.ID, Name = p.Name }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLanguages()
        {

            return Json(db.languages.Select(c => new { ID = c.ID, Name = c.Lang_Name }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getorgs()
        {

            return Json(db.organaizations.Select(c => new { OId = c.ID.ToString(), OName = c.Name }), JsonRequestBehavior.AllowGet);
        }
        


        public JsonResult GetTempType()
        {

            return Json(db.template_type.Select(c => new { ID = c.ID, Name = c.Name }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTempTypeOHCHR()
        {
            var Item = db.template_type.AsQueryable();

            
                Item = Item.Where(p => p.ID == 5);
        
            return Json(Item.Select(c => new { ID = c.ID, Name = c.Name }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTempTypeECE()
        {
            var Item = db.template_type.AsQueryable();

            // add id for ece
            Item = Item.Where(p => p.ID == 5);

            return Json(Item.Select(c => new { ID = c.ID, Name = c.Name }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCascadeCommittees(int? t)
        {
            
            var Item = db.committees.AsQueryable();

            if (t != null)
            {
                Item = Item.Where(p => p.template_type_ID == t);
            }

            return Json(Item.Select(p => new { ID = p.ID, Name = p.Name }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCascadeCommitteesECE()
        {

            var Item = db.committees.AsQueryable();

            Item = Item.Where(p => p.template_type_ID == 3);
            

            return Json(Item.Select(p => new { ID = p.ID, Name = p.Name }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCascadeDocT(int? comm)
        {
            
            var Item = db.doc_type.AsQueryable();

            if (comm != null)
            {
                Item = Item.Where(p => p.committee_ID == comm);
            }

            return Json(Item.Select(p => new { ID = p.ID, Name = p.Name }), JsonRequestBehavior.AllowGet);
        }


       public JsonResult GetCascadeCat(int? tt , string Filter)
        {

            ////var Item = db.categories.AsQueryable();
            //////tt = 8;
            ////if (tt != null)
            ////{
            ////    Item = Item.Where(p => p.type_ID == tt);
            ////}
            ////if (tt != null)
            ////{
            //   var  Item = db.categories.Where(x => x.type_ID == tt);
            ////}
            ////if (!string.IsNullOrEmpty(catfilter))
            ////{
            ////    Item = Item.Where(p => p.Name.Contains(catfilter));
            ////}
            //    return Json(Item.Select(p => new { CID = p.ID, CName = p.Name }), JsonRequestBehavior.AllowGet);
           
            var item = db.categories.AsQueryable();

            if (tt != null)
            {
              item   = item.Where(p => p.type_ID == tt);
            }

            if (!string.IsNullOrEmpty(Filter))
            {
                item = item.Where(p => p.Name.Contains(Filter));
            }

            return Json(item.Select(p => new { CID = p.ID, CName = p.Name }), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetCascadeSCat(int? tt, string Filter)
        {

       //     int id=-1;
            var item = db.final_temp.AsQueryable();

            if (tt != null)
            {
                item = item.Where(p => p.categories_ID == tt );

                //item = item.Where(p => p.Title != p.category.Name);

            }

            //foreach (var it in item)
            //{
            //    if (it.Title == it.category.Name)
            //    {
            //        id = it.ID;
            //    }
               
            //}
      
            if (!string.IsNullOrEmpty(Filter))
            {
                item = item.Where(p => p.Name.Contains(Filter));
            }

            //if (id >=0)
            //{
            //    item.Where(p => p.ID == id).Select(x => x.Title == "Not Applicable");
            //}




            return Json(item.Select(p => new { SCName = p.Title }), JsonRequestBehavior.AllowGet);

        }

        // GET: Helper
        public ActionResult Index()
        {
            return View();
        }
    }
}