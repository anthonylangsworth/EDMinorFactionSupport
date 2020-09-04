using EDMissionSummary.SummaryEntries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary.JournalEntryProcessors
{
    public class FsdJumpEventProcessor : JournalEntryProcessor
    {
        public override IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JObject entry)
        {
            throw new NotImplementedException();
        }
    }
}
