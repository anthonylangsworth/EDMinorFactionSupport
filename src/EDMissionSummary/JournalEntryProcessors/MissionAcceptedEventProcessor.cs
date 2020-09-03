using EDMissionSummary.SummaryEntries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMissionSummary.JournalEntryProcessors
{
    public class MissionAcceptedEventProcessor : JournalEntryProcessor
    {
        public static readonly string EventName = "MissionAccepted";

        public override IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedFaction, JObject entry)
        {
            if (pilotState is null)
            {
                throw new ArgumentNullException(nameof(pilotState));
            }
            if (supportedFaction is null)
            {
                throw new ArgumentNullException(nameof(supportedFaction));
            }
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            base.Process(pilotState, galaxyState, supportedFaction, entry);

            pilotState.Missions.Add(new Mission(
                entry.Value<string>("MissionID"),
                entry.Value<string>("LocalisedName"),
                entry.Value<string>("Faction"),
                entry.Value<string>("TargetFaction"),
                entry.Value<string>("DestinationSystem"),
                entry.Value<string>("DestinationStation")
            ));

            return Enumerable.Empty<SummaryEntry>();
        }
    }
}
