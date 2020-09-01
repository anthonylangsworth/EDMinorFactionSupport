using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using EDMissionSummary.SummaryEntries;

namespace EDMissionSummary.JournalEntryProcessors
{
    /// <summary>
    /// Base class for classes that process a event type in the journal.
    /// </summary>
    public abstract class JournalEntryProcessor
    {
        public abstract SummaryEntry Process(PilotState pilotState, SupportedFaction supportedFaction, JObject entry);
    }
}
