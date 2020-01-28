using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Replacing;
using Aspose.Words.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public static class StringExt
    {
        private static double _;

        public static bool IsNumeric(this string text) => double.TryParse(text, out _);

    }
    public class AutoFormattingController : Controller
    {
        // GET: AutoFormatting
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Format(string name)
        {
            string source = Server.MapPath(Path.Combine("/", "TestFormat/Test.docx"));
            //string download = DocVremoveP(source);
            PageOrientation(source);
            return File(Url.Content(source), "text/plain", "Testf" + ".docx");
        }
        public static void PageOrientation(string source)
        {
            Document doc = new Document(source);
            //DocumentBuilder builder = new DocumentBuilder(doc);
            //Shape textbox = new Shape(doc, ShapeType.TextBox);
            //textbox.Height = 481.89;
            //textbox.Width = 17.0079;
            //textbox.RelativeVerticalPosition = RelativeVerticalPosition.Page;
            //textbox.TextBox.InternalMarginBottom = 0;
            //textbox.TextBox.InternalMarginLeft = 0;
            //textbox.TextBox.InternalMarginRight = 0;
            //textbox.TextBox.InternalMarginTop = 0;
            //textbox.TextBox.LayoutFlow = LayoutFlow.Vertical;

            foreach (Section section in doc.Sections)
            {
                if (section.PageSetup.Orientation.ToString() == "Landscape")
                {
                    //section.HeadersFooters.LinkToPrevious(false);
          
                    //HeaderFooterCollection headersFooters = section.HeadersFooters;
                    //headersFooters.LinkToPrevious(false);
                    //string x = header.GetText();
                    //section.PageSetup.DifferentFirstPageHeaderFooter = false;
                    //header.IsLinkedToPrevious = false;
                    
                    //section.PageSetup.OddAndEvenPagesHeaderFooter = true;

                    //section.PageSetup.OddAndEvenPagesHeaderFooter = true;
                    ////header.IsLinkedToPrevious = false;
                    //string x = header.GetText();
                    //header.Remove();
                    //Paragraph paragraph = new Paragraph(doc);
                    //paragraph.AppendChild(new Run(doc, x));
                    //textbox.AppendChild(paragraph);
                    //builder.MoveToHeaderFooter(HeaderFooterType.HeaderFirst);
                    //builder.InsertNode(textbox);
                    ////header.AppendChild(textbox);
                    doc.Save(source);
                }
            }
        }

    }

}
