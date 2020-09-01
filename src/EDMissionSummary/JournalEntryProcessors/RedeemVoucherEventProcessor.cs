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

        public override IEnumerable<SummaryEntry> Process(PilotState pilotState, SupportedFaction supportedFaction, JObject entry)
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

            base.Process(pilotState, supportedFaction, entry);

            List<SummaryEntry> result = new List<SummaryEntry>();
            if (entry.Value<string>(TypePropertyName) == BountyValue)
            {
                JToken supportedFactionBounty = entry.Value<JArray>(FactionsPropertyName)
                                                      .FirstOrDefault(e => ((JObject) e).Value<string>("Faction") == supportedFaction.Name);
                if (supportedFactionBounty != null)
                {
                    result.Add(new BountySummaryEntry(supportedFactionBounty.Value<string>("Amount")));
                }
            }

            return result;
        }
    }
}
