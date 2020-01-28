using Aspose.Words;
using Aspose.Words.Replacing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentitySample.Controllers
{
    public class FindAndInsertHyperlink : IReplacingCallback
    {

        string linktext;
        string link;
        DocumentBuilder builder;
        public FindAndInsertHyperlink(string text, string link)
        {
            linktext = text;
            this.link = link;
        }
        ReplaceAction IReplacingCallback.Replacing(ReplacingArgs e)
        {
            // This is a Run node that contains either the beginning or the complete match.
            Node currentNode = e.MatchNode;

            if (builder == null)
                builder = new DocumentBuilder((Document)currentNode.Document);

            // The first (and may be the only) run can contain text before the match, 
            // in this case it is necessary to split the run.
            if (e.MatchOffset > 0)
                currentNode = SplitRun((Run)currentNode, e.MatchOffset);

            ArrayList runs = new ArrayList();

            // Find all runs that contain parts of the match string.
            int remainingLength = e.Match.Value.Length;
            while (
                (remainingLength > 0) &&
                (currentNode != null) &&
                (currentNode.GetText().Length <= remainingLength))
            {
                runs.Add(currentNode);
                remainingLength = remainingLength - currentNode.GetText().Length;

                // Select the next Run node. 
                // Have to loop because there could be other nodes such as BookmarkStart etc.
                do
                {
                    currentNode = currentNode.NextSibling;
                }
                while ((currentNode != null) && (currentNode.NodeType != NodeType.Run));
            }

            // Split the last run that contains the match if there is any text left.
            if ((currentNode != null) && (remainingLength > 0))
            {
                SplitRun((Run)currentNode, remainingLength);
                runs.Add(currentNode);
            }

            Run run = (Run)runs[0];
            if (run.ParentNode != null)
            {
                builder.MoveTo(run);
                builder.Font.Name = "Times New Roman";
                builder.Font.Color = System.Drawing.Color.Blue;
                builder.InsertHyperlink(e.Match.Value, link + e.Match.Value, false);
                builder.Font.ClearFormatting();
                foreach (Run node in runs)
                {
                    node.Remove();
                }
            }
           
            // Signal to the replace engine to do nothing because we have already done all what we wanted.
            return ReplaceAction.Skip;
        }

        /// <summary>
        /// Splits text of the specified run into two runs.
        /// Inserts the new run just after the specified run.
        /// </summary>
        private static Run SplitRun(Run run, int position)
        {
            Run afterRun = (Run)run.Clone(true);
            if (run.Text.Length > position)
            {
                afterRun.Text = run.Text.Substring(position);
                run.Text = run.Text.Substring(0, position);
                run.ParentNode.InsertAfter(afterRun, run);
            }
           
            
            return afterRun;
        }
    }
}