using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentitySample
{
    public class test
    {
        //public static void CleanStyle(string source)
        //{
        //    Document doc = new Document(source);

        //    CleanupOptions cleanupoptions = new CleanupOptions();
        //    cleanupoptions.UnusedLists = false;
        //    cleanupoptions.UnusedStyles = true;


        //    // Cleans unused styles and lists from the document depending on given CleanupOptions. 
        //    doc.Cleanup(cleanupoptions);
        //    doc.Save(source);
        //}
        //public static void CleanDoc(string source)
        //{
        //    Document doc = new Document(source);
        //    // Remove all empty paragraphs.

        //    Node[] paragraphs = doc.GetChildNodes(NodeType.Paragraph, true).ToArray();
        //    foreach (Paragraph paragraph in paragraphs)

        //    {
        //        //reset user Applied formatting
        //        foreach (Run run in paragraph.Runs)
        //        {
        //            run.Font.Name = "Times New Roman";
        //            run.Font.Size = 10;
        //            run.Font.Color = System.Drawing.Color.Black;
        //        }
        //        //doc.UpdatePageLayout();
        //        //change style used to Normal
        //        paragraph.ParagraphFormat.Style = doc.Styles["SingleT"];
        //        doc.UpdatePageLayout();
        //    }
        //    doc.Range.Replace(ControlChar.LineBreak, " ", new FindReplaceOptions());
        //    //Check if paragraph is empty.
        //    foreach (Paragraph paragraph in paragraphs)
        //    {
        //        if (string.IsNullOrEmpty(paragraph.GetText().Trim()) || (paragraph.Runs.Count == 0))
        //        {
        //            paragraph.Remove();
        //        }
        //    }
        //    //Remove Styles


        //    doc.Save(source);
        //}

        //public static void SingleText(string source)
        //{
        //    Document doc = new Document(source);

        //    // Create a paragraph style and specify some formatting for it.
        //    //Style style = doc.Styles.Add(StyleType.Paragraph, "SingleTextAuto");
        //    // style.Font.Size = 10;
        //    // style.Font.Name = "Times New Roman";
        //    //style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
        //    //style.ParagraphFormat.LineSpacingRule = LineSpacingRule.AtLeast;
        //    //style.ParagraphFormat.LineSpacing = 12;
        //    //style.ParagraphFormat.SpaceBefore = 0;
        //    //style.ParagraphFormat.SpaceAfter = 6;
        //    //style.ParagraphFormat.LeftIndent = 56.6929;
        //    //style.ParagraphFormat.RightIndent = 56.6929;
        //    //style.ParagraphFormat.NoSpaceBetweenParagraphsOfSameStyle = false;
        //    //style.ParagraphFormat.SpaceAfterAuto = false;
        //    //style.ParagraphFormat.SpaceBeforeAuto = false;
        //    //style.ParagraphFormat.FirstLineIndent = 56.6929;
        //    //doc.Save(source);



        //    // Get all paragraphs
        //    NodeCollection paragraphs = doc.GetChildNodes(NodeType.Paragraph, true);
        //    // Loop through all paragraphs
        //    //for (int parIdx = 0; parIdx < paragraphs.Count(); parIdx++)
        //    //{
        //    //    // Get paragraph
        //    //    Paragraph par = (Paragraph)paragraphs[parIdx];
        //    //    // Check if paragraph is child of table.
        //    //    // And exclude such paragraphs from further processing.
        //    //    if (par.GetAncestor(NodeType.Table) != null)
        //    //        continue;

        //    //    foreach (Run run in par.Runs)
        //    //    {
        //    //        run.Font.ClearFormatting();
        //    //    }
        //    //    par.ParagraphBreakFont.ClearFormatting();
        //    //    par.ParagraphFormat.StyleName = "SingleTextAuto";
        //    //    par.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
        //    //    par.ParagraphFormat.LineSpacingRule = LineSpacingRule.AtLeast;
        //    //    par.ParagraphFormat.LineSpacing = 12;
        //    //    par.ParagraphFormat.SpaceBefore = 0;
        //    //    par.ParagraphFormat.SpaceAfter = 6;
        //    //    par.ParagraphFormat.LeftIndent = 56.6929;
        //    //    par.ParagraphFormat.RightIndent = 56.6929;
        //    //    par.ParagraphFormat.NoSpaceBetweenParagraphsOfSameStyle = false;
        //    //    par.ParagraphFormat.SpaceAfterAuto = false;
        //    //    par.ParagraphFormat.SpaceBeforeAuto = false;
        //    //}

        //    foreach (Paragraph para in doc.GetChildNodes(NodeType.Paragraph, true))
        //    {


        //        para.ParagraphFormat.Style = doc.Styles["SingleT"]; ;

        //        //para.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
        //        //para.ParagraphFormat.LineSpacingRule = LineSpacingRule.AtLeast;
        //        //para.ParagraphFormat.LineSpacing = 12;
        //        //para.ParagraphFormat.SpaceBefore = 0;
        //        //para.ParagraphFormat.SpaceAfter = 6;
        //        //para.ParagraphFormat.LeftIndent = 56.6929;
        //        //para.ParagraphFormat.RightIndent = 56.6929;
        //        //para.ParagraphFormat.NoSpaceBetweenParagraphsOfSameStyle = false;
        //        //para.ParagraphFormat.SpaceAfterAuto = false;
        //        //para.ParagraphFormat.SpaceBeforeAuto = false;
        //        //foreach (Run run in para.Runs)
        //        //{
        //        //    run.Font.ClearFormatting();
        //        //}
        //        //para.ParagraphBreakFont.ClearFormatting();
        //    }
        //    doc.UpdatePageLayout();
        //    doc.Save(source);
        //}
        //public static void FormatTable(string source)
        //{
        //    Document doc = new Document(source);
        //    double width = 0;

        //    TableStyle tableStyle = (TableStyle)doc.Styles.Add(StyleType.Table, "NumStyle");
        //    tableStyle.ConditionalStyles.FirstColumn.ParagraphFormat.Alignment = ParagraphAlignment.Left;
        //    tableStyle.ConditionalStyles.FirstRow.Borders.Top.LineWidth = 0.5;
        //    tableStyle.ConditionalStyles.FirstRow.Borders.Bottom.LineWidth = 1.5;
        //    tableStyle.ConditionalStyles.FirstRow.ParagraphFormat.SpaceBefore = 4;
        //    tableStyle.ConditionalStyles.FirstRow.ParagraphFormat.SpaceAfter = 4;
        //    tableStyle.ConditionalStyles.FirstRow.ParagraphFormat.LineSpacingRule = LineSpacingRule.Exactly;
        //    tableStyle.ConditionalStyles.FirstRow.ParagraphFormat.LineSpacing = 10;
        //    tableStyle.ConditionalStyles.FirstRow.Font.Size = 8;
        //    tableStyle.ConditionalStyles.FirstRow.Font.Name = "Times New Roman";
        //    tableStyle.ConditionalStyles.FirstRow.Font.Italic = true;
        //    tableStyle.Font.Name = "Times New Roman";
        //    tableStyle.Font.Size = 9;
        //    tableStyle.Font.Bold = false;
        //    tableStyle.Font.Italic = false;
        //    tableStyle.ParagraphFormat.LineSpacingRule = LineSpacingRule.Exactly;
        //    tableStyle.ParagraphFormat.LineSpacing = 11;
        //    tableStyle.ParagraphFormat.SpaceBefore = 2;
        //    tableStyle.ParagraphFormat.SpaceAfter = 2;
        //    tableStyle.ParagraphFormat.Alignment = ParagraphAlignment.Right;
        //    tableStyle.Borders.Bottom.LineWidth = 1.5;



        //    //Make table autofit with content
        //    foreach (Table table in doc.GetChildNodes(NodeType.Table, true))
        //    {
        //        table.AutoFit(AutoFitBehavior.AutoFitToContents);
        //        doc.Save(source);
        //    }

        //    //decide on table width
        //    foreach (Table table in doc.GetChildNodes(NodeType.Table, true))
        //    {
        //        foreach (Cell cell in table.FirstRow.Cells)

        //        {

        //            width += cell.CellFormat.Width;
        //            cell.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom;

        //        }
        //        if (width <= 368)
        //        {
        //            table.PreferredWidth = PreferredWidth.FromPoints(368.504);
        //            table.StyleName = "NumStyle";

        //        }
        //        if (width <= 425 & width > 368)
        //        {
        //            table.PreferredWidth = PreferredWidth.FromPoints(425.197);
        //            table.StyleName = "NumStyle";
        //        }
        //        if (width >= 481)
        //        {
        //            table.PreferredWidth = PreferredWidth.FromPoints(481.89);
        //            table.StyleName = "NumStyle";
        //        }
        //        doc.Save(source);
        //        string text = table.Range.Text;
        //    }
        //    doc.Save(source);
        //}

    }
}