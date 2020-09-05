using EDMinorFactionSupport.SummaryEntries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMinorFactionSupport.JournalEntryProcessors
{
    public class FsdJumpEntryProcessor : JournalEntryProcessor
    {
        public static readonly string EventName = "FSDJump";

        public override IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JObject entry)
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
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            galaxyState.Systems[entry.Value<long>("SystemAddress")] = new StarSystem(
                    entry.Value<long>("SystemAddress"),
                    entry.Value<string>("StarSystem"),
                    entry["Factions"]?.Select(o => o.Value<string>("Name")));

            return Enumerable.Empty<SummaryEntry>();
        }
    }
}
