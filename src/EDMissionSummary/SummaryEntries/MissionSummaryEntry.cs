using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary.SummaryEntries
{
    public class MissionSummaryEntry: SummaryEntry
    {
        public MissionSummaryEntry(DateTime timestamp, string name, string systemName, bool increasesInfluence, string influence)
            : base(timestamp, systemName, increasesInfluence)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentException($"'{nameof(systemName)}' cannot be null or whitespace", nameof(systemName));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }
            if (string.IsNullOrWhiteSpace(influence))
            {
                throw new ArgumentException($"'{nameof(influence)}' cannot be null or whitespace", nameof(influence));
            }

            Influence = influence;
            Name = name;
        }

        public string Influence { get; }

        public string Name { get; }

        public override string ToString()
        {
            return string.Format("{0}: {1} {2} Inf{3} '{4}'", TimeStamp, SystemName, IncreasesInfluence ? "Pro" : "Con", Influence, Name);
        }
    }
}
