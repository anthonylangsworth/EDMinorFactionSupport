using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using EDMissionSummary.SummaryEntries;
using System.Globalization;
using System.Linq;

namespace EDMissionSummary.JournalEntryProcessors
{
    /// <summary>
    /// Base class for classes that process a event type in the journal.
    /// </summary>
    public abstract class JournalEntryProcessor
    {
        public DateTime TimeStamp
        {
            get;
            private set;
        }

        public virtual IEnumerable<SummaryEntry> Process(PilotState pilotState, string supportedFaction, JObject entry)
        {
            TimeStamp = DateTime.Parse(entry.Value<string>("timestamp"), CultureInfo.CurrentUICulture);

            return Enumerable.Empty<SummaryEntry>();
        }
    }
}
