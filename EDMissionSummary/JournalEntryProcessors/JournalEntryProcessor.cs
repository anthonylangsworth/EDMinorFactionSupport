using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary.JournalEntryProcessors
{
    public abstract class JournalEntryProcessor
    {
        public abstract SquadronSummaryEntry Process(PilotState pilotState, SupportedFaction supportedFaction, JObject entry);
    }
}
