using EDMinorFactionSupport.SummaryEntries;
using NSW.EliteDangerous.API.Events;
using NSW.EliteDangerous.API.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMinorFactionSupport.JournalEntryProcessors
{
    public class FsdJumpEntryProcessor : JournalEventProcessor
    {
        public override string EventName => "FSDJump";

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

            FsdJumpEvent fsdJumpEvent = (FsdJumpEvent) journalEvent;

            galaxyState.Systems[fsdJumpEvent.SystemAddress] = new StarSystem(
                    fsdJumpEvent.SystemAddress,
                    fsdJumpEvent.StarSystem,
                    fsdJumpEvent.Factions.Select(faction => faction.Name));

            return Enumerable.Empty<SummaryEntry>();
        }
    }
}
