using Aspose.Words;
using Aspose.Words.Replacing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentitySample.Controllers
{
    public class urlReplaceEvaluator : IReplacingCallback
    {
        public ReplaceAction Replacing(ReplacingArgs args)

        {

            // This is a Run node that contains either the beginning or the complete match.

            Node currentNode = args.MatchNode;


            // The first (and may be the only) run can contain text before the match,

            // in this case it is necessary to split the run.

            if (args.MatchOffset > 0)

                currentNode = SplitRun((Run)currentNode, args.MatchOffset);


            // This array is used to store all nodes of the match for further removing.

            ArrayList runs = new ArrayList();


            // Get url.

            string url = args.Match.Value;


            // Find all runs that contain parts of the match string.

            int remainingLength = args.Match.Value.Length;

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



            // Create DocumentBuilder and move its cursor to the match node.

            DocumentBuilder builder = new DocumentBuilder((Document)args.MatchNode.Document);

            builder.MoveTo((Node)runs[0]);


            // Insert Hyperlink

            builder.Font.StyleIdentifier = StyleIdentifier.Hyperlink;

            builder.InsertHyperlink(url, url, false);


            // Now remove all runs in the sequence.

            foreach (Run run in runs)

                run.Remove();


            // Signal to the replace engine to do nothing because we have already done all what we wanted.

            return ReplaceAction.Skip;

        }


        ///

        /// Splits text of the specified run into two runs.

        /// Inserts the new run just after the specified run.

        ///

        private static Run SplitRun(Run run, int position)

        {

            Run afterRun = (Run)run.Clone(true);

            afterRun.Text = run.Text.Substring(position);

            run.Text = run.Text.Substring(0, position);

            run.ParentNode.InsertAfter(afterRun, run);

            return afterRun;

        }

    }
}