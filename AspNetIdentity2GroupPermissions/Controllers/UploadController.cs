using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveE(IEnumerable<HttpPostedFileBase> filesE, string fileDescription)
        {
            string name = "";
            // The Name of the Upload component is "files"
            if (filesE != null)
            {
                foreach (var file in filesE)
                {
                    
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "E" + fileExtention;
                    name = fileName;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // The files are not actually saved in this demo
                     file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            //return Content("");
            return Json(new { newName = name }, "text/plain" );
            //return Json(new { ImageName = newImageName }, "text/plain");
        }

        public ActionResult RemoveE(string[] fileNames, string fileDescription)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "E" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                         System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult SaveA(IEnumerable<HttpPostedFileBase> filesA, string fileDescription)
        {
            string name = "";
            // The Name of the Upload component is "files"
            if (filesA != null)
            {
                foreach (var file in filesA)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "A" + fileExtention;
                    name = fileName;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            //return Content("");
            return Json(new { newName = name }, "text/plain");
            //return Json(new { ImageName = newImageName }, "text/plain");
        }

        public ActionResult RemoveA(string[] fileNames, string fileDescription)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "A" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult SaveF(IEnumerable<HttpPostedFileBase> filesF, string fileDescription)
        {
            // The Name of the Upload component is "files"
            if (filesF != null)
            {
                foreach (var file in filesF)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "F" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
            //return Json(new { ImageName = newImageName }, "text/plain");
        }

        public ActionResult RemoveF(string[] fileNames, string fileDescription)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "F" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult SaveC(IEnumerable<HttpPostedFileBase> filesC, string fileDescription)
        {
            // The Name of the Upload component is "files"
            if (filesC != null)
            {
                foreach (var file in filesC)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "C" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
            //return Json(new { ImageName = newImageName }, "text/plain");
        }

        public ActionResult RemoveC(string[] fileNames, string fileDescription)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "C" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult SaveR(IEnumerable<HttpPostedFileBase> filesR, string fileDescription)
        {
            // The Name of the Upload component is "files"
            if (filesR != null)
            {
                foreach (var file in filesR)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "R" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
            //return Json(new { ImageName = newImageName }, "text/plain");
        }

        public ActionResult RemoveR(string[] fileNames, string fileDescription)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "R" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult SaveS(IEnumerable<HttpPostedFileBase> filesS, string fileDescription)
        {
            // The Name of the Upload component is "files"
            if (filesS != null)
            {
                foreach (var file in filesS)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "S" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
            //return Json(new { ImageName = newImageName }, "text/plain");
        }

        public ActionResult RemoveS(string[] fileNames, string fileDescription)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "F" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult SaveSR(IEnumerable<HttpPostedFileBase> filesSR, string fileDescription)
        {
            // The Name of the Upload component is "files"
            if (filesSR != null)
            {
                foreach (var file in filesSR)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "E" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
            //return Json(new { ImageName = newImageName }, "text/plain");
        }
        public ActionResult RemoveSR(string[] fileNames, string fileDescription)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "SRE" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult SaveSRR(IEnumerable<HttpPostedFileBase> filesSRR, string fileDescription)
        {
            // The Name of the Upload component is "files"
            if (filesSRR != null)
            {
                foreach (var file in filesSRR)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "RE" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
            //return Json(new { ImageName = newImageName }, "text/plain");
        }
        public ActionResult RemoveSRR(string[] fileNames, string fileDescription)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    string fileExtention = fileName.Substring(fileName.LastIndexOf("."));
                    fileName = fileDescription + "SRRE" + fileExtention;
                    var physicalPath = Path.Combine(Server.MapPath("~/IN"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Async_Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Meta"), fileName);

                    // The files are not actually saved in this demo
                     file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Async_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Meta"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                         System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
    }
}