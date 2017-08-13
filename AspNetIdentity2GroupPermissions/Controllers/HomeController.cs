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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        private gdgs1Entities db = new gdgs1Entities();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private DownloadRepo Repo = new DownloadRepo();
        
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Super_Admin"))
            {
                return RedirectToAction("DashBoard", "Home");
            }
            if (User.IsInRole("OHCHR_Admin"))
            {
                return RedirectToAction("DashBoard", "Home");
            }
            if (User.IsInRole("Unog_Admin"))
            {
                return RedirectToAction("DashBoard", "Home");
            }
            if (User.IsInRole("OHCHR_User"))
            {
                return RedirectToAction("Index", "OHCHR");
            }
            if (User.IsInRole("Unog_User"))
            {
                return RedirectToAction("Index", "UNOG");
            }
            if (User.IsInRole("Template_Admin"))
            {
                return RedirectToAction("DashBoard", "Home");
            }
            else { return RedirectToAction("DashBoard", "Home"); }
          
        }
        public ActionResult DashBoard(string searchString)
        {
            var result = Repo.Read();
            int count = 0;
            var userslist = UserManager.Users.ToList();

            int users = userslist.Count();
            int types = db.template_type.ToList().Count(); ;
            int entities =db.committees.ToList().Count();
            int template = db.final_temp.ToList().Count(); ;

            ViewBag.U = users;
            ViewBag.E = entities;
            ViewBag.T = types;
            ViewBag.temp = template;

            if (!String.IsNullOrEmpty(searchString))
            {
                count = 1;
            }
            ViewBag.Count = count;

            return View(result);
        }
        public ActionResult downloadE(string name)
        {
            name = name + "E";
            try
            {
                return File(Url.Content("/GDGS/IN/" + name + ".docx"), "text/plain", name + ".docx");
            }
            catch
            {
                return RedirectToAction("WrongTemplate", "Unog");
            }
        }
        public ActionResult downloadF(string name)
        {
            name = name + "F";
            try
            {
                return File(Url.Content("/GDGS/IN/" + name + ".docx"), "text/plain", name + ".docx");
            }
            catch
            {
                return RedirectToAction("WrongTemplate", "Unog");
            }
        }
        public ActionResult downloadS(string name)
        {
            name = name + "S";
            try
            {
                return File(Url.Content("/GDGS/IN/" + name + ".docx"), "text/plain", name + ".docx");
            }
            catch
            {
                return RedirectToAction("WrongTemplate", "Unog");
            }
        }

    }
}
