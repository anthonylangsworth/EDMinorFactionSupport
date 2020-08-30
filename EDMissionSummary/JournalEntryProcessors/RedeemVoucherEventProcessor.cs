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

        public override SquadronSummaryEntry Process(PilotState pilotState, SupportedFaction supportedFaction, JObject entry)
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

            BountySummaryEntry result = null;
            if (entry.Value<string>(TypePropertyName) == BountyValue)
            {
                JToken supportedFactionBounty = entry.Value<JArray>(FactionsPropertyName)
                                                      .FirstOrDefault(e => ((JObject) e).Value<string>("Faction") == supportedFaction.Name);
                if (supportedFactionBounty != null)
                {
                    result = new BountySummaryEntry(supportedFactionBounty.Value<string>("Amount"));
                }
            }

            return result;
        }
    }
}
