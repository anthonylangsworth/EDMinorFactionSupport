using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EDMissionSummary
{
    public abstract  class JournalSource
    {
        public abstract IEnumerable<string> Entries
        {
            get;
        }
    }
}
