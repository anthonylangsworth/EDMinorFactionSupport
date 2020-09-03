using EDMissionSummary.SummaryEntries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMissionSummary.JournalEntryProcessors
{
    internal class RedeemVoucherEventProcessor : JournalEntryProcessor
    {
        public static readonly string EventName = "RedeemVoucher";
        public static readonly string TypePropertyName = "Type";
        public static readonly string BountyValue = "bounty";
        public static readonly string FactionsPropertyName = "Factions";

        public override IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JObject entry)
        {
            if (pilotState is null)
            {
                throw new ArgumentNullException(nameof(pilotState));
            }
            if (supportedMinorFaction is null)
            {
                throw new ArgumentNullException(nameof(supportedMinorFaction));
            }
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            List<SummaryEntry> result = new List<SummaryEntry>();
            if (entry.Value<string>(TypePropertyName) == BountyValue)
            {
                JToken supportedMinorFactionBounty = entry.Value<JArray>(FactionsPropertyName)
                                                      .FirstOrDefault(e => ((JObject) e).Value<string>("Faction") == supportedMinorFaction);
                if (supportedMinorFactionBounty != null)
                {
                    result.Add(new BountySummaryEntry(GetTimeStamp(entry), supportedMinorFactionBounty.Value<string>("Amount")));
                }
            }

            return result;
        }
    }
}
