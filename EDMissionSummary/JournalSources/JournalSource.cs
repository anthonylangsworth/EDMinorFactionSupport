using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EDMissionSummary.JournalSources
{
    public abstract class JournalSource
    {
        public abstract IEnumerable<string> Entries
        {
            get;
        }
    }
}
