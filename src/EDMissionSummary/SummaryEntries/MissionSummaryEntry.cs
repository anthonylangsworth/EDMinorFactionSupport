using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary.SummaryEntries
{
    public class MissionSummaryEntry: SummaryEntry
    {
        public MissionSummaryEntry(DateTime timestamp, string systemName, bool increasesInfluence, string influence)
            : base(timestamp, systemName, increasesInfluence)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentException($"'{nameof(systemName)}' cannot be null or whitespace", nameof(systemName));
            }

            if (string.IsNullOrWhiteSpace(influence))
            {
                throw new ArgumentException($"'{nameof(influence)}' cannot be null or whitespace", nameof(influence));
            }

            Influence = influence;
        }

        public string Influence { get; }

        public override string ToString()
        {
            return string.Format("{0} {1} Inf{2}", SystemName, IncreasesInfluence ? "Pro" : "Con", Influence);
        }
    }
}
