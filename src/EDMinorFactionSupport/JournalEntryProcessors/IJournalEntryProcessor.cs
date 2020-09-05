using EDMinorFactionSupport.SummaryEntries;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EDMinorFactionSupport.JournalEntryProcessors
{
    public interface IJournalEntryProcessor
    {
        string EventName { get; }

        IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JObject entry);
    }
}