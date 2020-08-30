using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary
{
    public class SquadronSummaryMissionEntry: SquadronSummaryEntry
    {
        public SquadronSummaryMissionEntry(string systemName, string supportedFaction, int influencePluses)
        {
            SystemName = systemName;
            SupportedFaction = supportedFaction;
            InfluencePluses = influencePluses;
        }

        public string SystemName { get; }
        public string SupportedFaction { get; }
        public int InfluencePluses { get; }
    }
}
