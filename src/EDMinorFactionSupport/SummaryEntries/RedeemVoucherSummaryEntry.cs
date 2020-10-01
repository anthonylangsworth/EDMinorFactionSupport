using NSW.EliteDangerous.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EDMinorFactionSupport.SummaryEntries
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class RedeemVoucherSummaryEntry : SummaryEntry, IEquatable<RedeemVoucherSummaryEntry>
    {
        /// <summary>
        /// Create a new <see cref="RedeemVoucherSummaryEntry"/>.
        /// </summary>
        /// <param name="timestamp">
        /// When the voucher was redeemed.
        /// </param>
        /// <param name="systemName">
        /// The system where the influece gain or loss occurred.
        /// </param>
        /// <param name="increasesInfluence">
        /// TYrue if this increases the minor faction's influence, false otherwise.
        /// </param>
        /// <param name="voucherType">
        /// The voucher type, as per the journal entry "type" field.
        /// </param>
        /// <param name="amount">
        /// The amount in credits. This must be positive.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="voucherType"/> cannot be null, empty or whitespace. <paramref name="amount"/> must be positive.
        /// </exception>
        public RedeemVoucherSummaryEntry(DateTime timestamp, string systemName, bool increasesInfluence, VoucherType voucherType, long amount)
            : base(timestamp, systemName, increasesInfluence)
        {
            if (Enum.IsDefined(typeof(VoucherType), voucherType))
            {
                throw new ArgumentException($"'{nameof(voucherType)}' is not valid", nameof(voucherType));
            }
            if (amount < 0)

            {
                throw new ArgumentException($"'{nameof(amount)}' cannot cannot be negative", nameof(amount));
            }

            VoucherType = voucherType;
            Amount = amount;
        }

        /// <summary>
        /// The voucher type, as per the journal entry "type" field.
        /// </summary>
        public VoucherType VoucherType { get; }

        /// <summary>
        /// The amount in credits.
        /// </summary>
        public long Amount { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as RedeemVoucherSummaryEntry);
        }

        public bool Equals(RedeemVoucherSummaryEntry other)
        {
            return other != null &&
                   base.Equals(other) &&
                   VoucherType == other.VoucherType &&
                   Amount == other.Amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), VoucherType, Amount);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} of {2:n0} CR", base.ToString(), VoucherType, Amount);
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
