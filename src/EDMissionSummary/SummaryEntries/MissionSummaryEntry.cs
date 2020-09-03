using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary.SummaryEntries
{
    public class MissionSummaryEntry: SummaryEntry
    {
        public MissionSummaryEntry(string systemName, bool supportsFaction, string influence)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentException($"'{nameof(systemName)}' cannot be null or whitespace", nameof(systemName));
            }

            if (string.IsNullOrWhiteSpace(influence))
            {
                throw new ArgumentException($"'{nameof(influence)}' cannot be null or whitespace", nameof(influence));
            }

            SystemName = systemName;
            SupportsFaction = supportsFaction;
            Influence = influence;
        }

        public string SystemName { get; }
        public bool SupportsFaction { get; }
        public string Influence { get; }

        public override string ToString()
        {
            return string.Format("{0} {1} Inf{2}", SystemName, SupportsFaction ? "Pro" : "Con", Influence);
        }
    }
}
