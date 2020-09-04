using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EDMissionSummary.SummaryEntries
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class RedeemVoucherSummaryEntry : SummaryEntry, IEquatable<RedeemVoucherSummaryEntry>
    {
        public RedeemVoucherSummaryEntry(DateTime timestamp, string voucherType, int amount)
            : base(timestamp)
        {
            VoucherType = voucherType;
            Amount = amount;
        }

        public string VoucherType { get; }
        public int Amount { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as RedeemVoucherSummaryEntry);
        }

        public bool Equals(RedeemVoucherSummaryEntry other)
        {
            return other != null &&
                   TimeStamp == other.TimeStamp &&
                   VoucherType == other.VoucherType &&
                   Amount == other.Amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TimeStamp, VoucherType, Amount);
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
