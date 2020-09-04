using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EDMissionSummary.SummaryEntries
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class BountySummaryEntry : SummaryEntry, IEquatable<BountySummaryEntry>
    {
        public BountySummaryEntry(DateTime timestamp, int amount)
            : base(timestamp)
        {
            Amount = amount;
        }

        public int Amount { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BountySummaryEntry);
        }

        public bool Equals(BountySummaryEntry other)
        {
            return other != null &&
                   TimeStamp == other.TimeStamp &&
                   Amount == other.Amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TimeStamp, Amount);
        }

        public override string ToString()
        {
            return string.Format("{0}: Bounty: {1} CR", TimeStamp, Amount);
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
