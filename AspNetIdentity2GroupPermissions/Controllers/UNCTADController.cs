using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class UNCTADController : Controller
    {
        // GET: UNCTAD
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult download()
        {
            return File(Url.Content("/GDGS/OUT/" + "UNCTAD-Research-Paper.docx"), "text/plain", "UNCTAD-Research-Paper.docx");
            //  return File(Url.Content("/OUT/" + name + ".docx"), "text/plain", name + ".docx");
        }
    }
}