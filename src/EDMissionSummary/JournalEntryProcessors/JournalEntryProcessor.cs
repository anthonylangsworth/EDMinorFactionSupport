using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using EDMissionSummary.SummaryEntries;
using System.Globalization;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace EDMissionSummary.JournalEntryProcessors
{
    /// <summary>
    /// Base class for classes that process a event type in the journal into <see cref="SummaryEntry"/> objects.
    /// </summary>
    public abstract class JournalEntryProcessor
    {
        /// <summary>
        /// Process the journal entry.
        /// </summary>
        /// <param name="pilotState">
        /// A <see cref="PilotState"/> representing data associated with the pilot, such as the current station or system.
        /// </param>
        /// <param name="galaxyState">
        /// A <see cref="GalaxyState"/> reoresenting the Elite: Dangerous universe the pilot plays in.
        /// </param>
        /// <param name="supportedMinorFaction">
        /// The supported minor faction name. This must <b>exactly</b> match the name in the journal.
        /// </param>
        /// <param name="entry">
        /// A <see cref="JObject"/> representing the journal entry.
        /// </param>
        /// <returns>
        /// Zero or more <see cref="SummaryEntry"/> objects representing actions that impact the supported minor faction.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// No argument can be null.
        /// </exception>
        public abstract IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JObject entry);

        /// <summary>
        /// Extract the date and time the journal entry was written.
        /// </summary>
        /// <param name="entry">
        /// The journal entry.
        /// </param>
        /// <returns>
        /// The date and time the journal entyr was written.
        /// </returns>
        protected DateTime GetTimeStamp(JObject entry)
        {
            return DateTime.Parse(entry.Value<string>("timestamp"), CultureInfo.CurrentUICulture);
        }
    }
}
