using EDMinorFactionSupport.JournalEntryProcessors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDMinorFactionSupport.SummaryEntries;
using NSW.EliteDangerous.API.Events;

namespace EDMinorFactionSupport
{
    public class Summarizer
    {
        /// <summary>
        /// Create a new <see cref="Summarizer"/>.
        /// </summary>
        /// <param name="journalEntryProcessors"></param>
        public Summarizer(IEnumerable<JournalEventProcessor> journalEntryProcessors)
        {
            if (journalEntryProcessors is null)
            {
                throw new ArgumentNullException(nameof(journalEntryProcessors));
            }

            JournalEntryProcessors = journalEntryProcessors.ToDictionary(jep => jep.EventName);
        }

        /// <summary>
        /// The <see cref="JournalEventProcessor"/> types used to process journal entries.
        /// </summary>
        public IReadOnlyDictionary<string, JournalEventProcessor> JournalEntryProcessors;

        public IEnumerable<SummaryEntry> Convert(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JournalEvent journalEvent)
        {
            if (pilotState is null)
            {
                throw new ArgumentNullException(nameof(pilotState));
            }
            if (supportedMinorFaction is null)
            {
                throw new ArgumentNullException(nameof(supportedMinorFaction));
            }
            if (journalEvent == null)
            {
                throw new NullReferenceException(nameof(journalEvent));
            }

            IEnumerable<SummaryEntry> result = Enumerable.Empty<SummaryEntry>();
            if (JournalEntryProcessors.TryGetValue(journalEvent.Event, out JournalEventProcessor journalEntryProcessor))
            {
                result = journalEntryProcessor.Process(pilotState, galaxyState, supportedMinorFaction, journalEvent);
            }
            return result;
        }
    }
}
