using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary
{
    public class SquadronSummaryMissionEntry: SquadronSummaryEntry
    {
        public SquadronSummaryMissionEntry(string sourceSystemId, string destinationSystem, bool supportingFaction, string factionInfluence)
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
    }
}
