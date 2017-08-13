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
    public class organaizationsController : Controller
    {
        private OrgRepo repo = new OrgRepo();

        // GET: organaizations
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(repo.Read().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, OrgViewModel org)
        {
            if (org != null && ModelState.IsValid)
            {
                repo.Create(org);
            }

            return Json(new[] { org }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, OrgViewModel org)
        {
            if (org != null && ModelState.IsValid)
            {
                repo.Update(org);
            }

            return Json(new[] { org }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, OrgViewModel org)
        {
            if (org != null)
            {
                repo.Destroy(org);
            }

            return Json(new[] { org }.ToDataSourceResult(request, ModelState));
        }
    }
}
