using EDMinorFactionSupport.SummaryEntries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EDMinorFactionSupport.OutputFormatters
{
    public abstract class OutputFormatter
    {
        public abstract void Format(IEnumerable<SummaryEntry> summary, TextWriter output);
    }
}
