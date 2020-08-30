using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary.SummaryEntries
{
    public class MissionSummaryEntry: SquadronSummaryEntry
    {
        public MissionSummaryEntry(string sourceSystemId, string destinationSystem, bool supportingFaction, string factionInfluence)
        {
            SourceSystemId = sourceSystemId;
            DestinationSystem = destinationSystem;
            SupportingFaction = supportingFaction;
            FactionInfluence = factionInfluence;
        }

        public string SourceSystemId { get; }
        public string DestinationSystem { get; }
        public bool SupportingFaction { get; }
        public string FactionInfluence { get; }

        public override string ToString()
        {
            return string.Format("{0} Inf{1}", SupportingFaction ? "Pro" : "Con", FactionInfluence);
        }
    }
}
