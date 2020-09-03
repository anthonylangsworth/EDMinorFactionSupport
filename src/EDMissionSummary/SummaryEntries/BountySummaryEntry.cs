using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary.SummaryEntries
{
    internal class BountySummaryEntry : SummaryEntry
    {
        public BountySummaryEntry(DateTime timestamp, string amount)
            : base(timestamp)
        {
            if (string.IsNullOrWhiteSpace(amount))
            {
                throw new ArgumentException($"'{nameof(amount)}' cannot be null or whitespace", nameof(amount));
            }

            Amount = amount;
        }

        public string Amount { get; }

        public override string ToString()
        {
            return string.Format("Bounty: {0} CR", Amount);
        }
    }
}
