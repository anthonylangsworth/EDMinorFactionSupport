using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary.SummaryEntries
{
    /// <summary>
    /// Base class for summary entries. These represent the processinng of the journal into relevant data in a display-independent way.
    /// </summary>
    public abstract class SummaryEntry
    {
        /// <summary>
        /// Create a new <see cref="SummaryEntry"/>.
        /// </summary>
        /// <param name="timeStamp">
        /// The date and time the entry was written.
        /// </param>
        protected SummaryEntry(DateTime timeStamp)
        {
            TimeStamp = timeStamp;
        }

        /// <summary>
        /// The date and time the entry was written.
        /// </summary>
        public DateTime TimeStamp { get; }
    }
}
