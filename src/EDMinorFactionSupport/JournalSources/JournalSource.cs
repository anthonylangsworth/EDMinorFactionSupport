using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EDMinorFactionSupport.JournalSources
{
    /// <summary>
    /// A source of Elite: Dangerous journal entries. 
    /// </summary>
    public abstract class JournalSource
    {
        /// <summary>
        /// The Elite: Dangerous journal entries.
        /// </summary>
        public abstract IEnumerable<string> Entries
        {
            get;
        }
    }
}
