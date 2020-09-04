using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EDMissionSummary.SummaryEntries
{
    /// <summary>
    /// Base class for summary entries. These represent the processinng of the journal into relevant data in a display-independent way.
    /// </summary>
    public abstract class SummaryEntry
    {
        /// <summary>
        /// Create a new <see cref="SummaryEntry"/>.
        /// </summary>
        /// <param name="timeStamp">
        /// The date and time the entry was written.
        /// </param>
        /// <param name="systemName">
        /// The name of the system where the influence change occurred. This may be null if not known.
        /// </param>
        /// <param name="increasesInfluence">
        /// True if the influence is an increase, false if a decrease,
        /// </param>
        protected SummaryEntry(DateTime timeStamp, string systemName, bool increasesInfluence)
        {
            TimeStamp = timeStamp;
            SystemName = systemName;
            IncreasesInfluence = increasesInfluence;
        }

        /// <summary>
        /// The date and time the entry was written.
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        /// The name of the system where the influence change occurred.
        /// </summary>
        public string SystemName { get; }

        /// <summary>
        /// True if this increases the minor faction's influence, false if it decreases it.
        /// </summary>
        public bool IncreasesInfluence { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SummaryEntry);
        }

        public bool Equals(SummaryEntry other)
        {
            return other != null &&
                   TimeStamp.Equals(other.TimeStamp) &&
                   SystemName == null ? other.SystemName == null : SystemName.Equals(other.SystemName) &&
                   IncreasesInfluence == other.IncreasesInfluence;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TimeStamp, SystemName, IncreasesInfluence);
        }

        public override string ToString()
        {
            return string.Format("{0} in '{1}' {2}:", TimeStamp.ToString("G", DateTimeFormatInfo.InvariantInfo), SystemName, IncreasesInfluence ? "PRO" : "CON");
        }
    }
}
