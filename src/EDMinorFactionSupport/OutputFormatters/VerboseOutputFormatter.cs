using EDMinorFactionSupport.SummaryEntries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDMinorFactionSupport.OutputFormatters
{
    public class VerboseOutputFormatter : OutputFormatter
    {
        public override void Format(IEnumerable<SummaryEntry> summary, TextWriter output)
        {
            if (summary is null)
            {
                throw new ArgumentNullException(nameof(summary));
            }
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            output.WriteLine(summary.Aggregate(new StringBuilder(), (sb, se) => sb.AppendLine(se.ToString())));
        }
    }
}
