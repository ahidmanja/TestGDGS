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

namespace IdentitySample.Controllers
{
    public class TempTypeController : Controller
    {
        private TempTypeRepo repo = new TempTypeRepo();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(repo.Read().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, TempTypeViewModel item)
        {
            if (item != null && ModelState.IsValid)
            {
                repo.Create(item);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, TempTypeViewModel item)
        {
            if (item != null && ModelState.IsValid)
            {
                repo.Update(item);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, TempTypeViewModel item)
        {
            if (item != null)
            {
                repo.Destroy(item);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }
    }
}