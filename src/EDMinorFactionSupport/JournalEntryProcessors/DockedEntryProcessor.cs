using EDMinorFactionSupport.SummaryEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSW.EliteDangerous.API.Events;

namespace EDMinorFactionSupport.JournalEntryProcessors
{
    public class DockedEntryProcessor : JournalEventProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        public override string EventName => "Docked";

        /// <summary>
        /// Track the mission in the <see cref="PilotState"/> because some mission relevant details are only
        /// available in this journal entry.
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
        /// <param name="journalEvent">
        /// A <see cref="JournalEvent"/> representing the journal entry.
        /// </param>
        /// <returns>
        /// Will never return <see cref="SummaryEntry"/> objects.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// No argument can be null.
        /// </exception>
        public override IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JournalEvent journalEvent)
        {
            if (pilotState is null)
            {
                throw new ArgumentNullException(nameof(pilotState));
            }
            if (galaxyState is null)
            {
                throw new ArgumentNullException(nameof(galaxyState));
            }
            if (supportedMinorFaction is null)
            {
                throw new ArgumentNullException(nameof(supportedMinorFaction));
            }
            if (journalEvent is null)
            {
                throw new ArgumentNullException(nameof(journalEvent));
            }

            DockedEvent dockedEvent = (DockedEvent)journalEvent;

            // Create a new station instead of looking it up because the "Docked" event may be received before the "Location" event.
            pilotState.LastDockedStation = new Station(
                dockedEvent.StationName,
                dockedEvent.SystemAddress,
                dockedEvent.StationFaction.Name
            );

            return Enumerable.Empty<SummaryEntry>();
        }
    }
}
