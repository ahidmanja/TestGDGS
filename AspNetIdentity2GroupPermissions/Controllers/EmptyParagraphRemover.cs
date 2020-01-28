using Aspose.Words;

namespace IdentitySample.Controllers
{
    internal class EmptyParagraphRemover: DocumentVisitor
    {
        public override VisitorAction VisitParagraphStart(Paragraph paragraph)
        {
            //paragraph.ParagraphFormat.ClearFormatting();
            if (paragraph.GetAncestor(NodeType.Table) == null)
            { paragraph.ParagraphFormat.StyleName = "_ Single Txt_G"; }
                
            if (string.IsNullOrEmpty(paragraph.GetText().Trim()) || (paragraph.Runs.Count == 0))
            {

                paragraph.Remove();
            }
            return VisitorAction.Continue;
        }
        public override VisitorAction VisitRun(Run run)
        {
            run.Font.Name = "Times New Roman";
            run.Font.Size = 10;
            run.Font.Color = System.Drawing.Color.Black;

            return VisitorAction.Continue;

        }
        //}
    }
}