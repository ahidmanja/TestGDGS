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
            
           
           Item = Item.Where(p => p.languages_ID == 1);

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

        public JsonResult GetCascadeCommittees(int? t)
        {
            
            var Item = db.committees.AsQueryable();

            if (t != null)
            {
                Item = Item.Where(p => p.template_type_ID == t);
            }

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

        // GET: Helper
        public ActionResult Index()
        {
            return View();
        }
    }
}