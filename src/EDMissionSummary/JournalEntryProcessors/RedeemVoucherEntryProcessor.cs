using EDMissionSummary.SummaryEntries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMissionSummary.JournalEntryProcessors
{
    public class RedeemVoucherEntryProcessor : JournalEntryProcessor
    {
        public static readonly string EventName = "RedeemVoucher";
        public static readonly string TypePropertyName = "Type";
        public static readonly string BountyValue = "bounty";
        public static readonly string FactionsPropertyName = "Factions";

        /// <summary>
        /// Add the bounty if it is relevant to the minor faction.
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
        /// Will never return <see cref="SummaryEntry"/> objects.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// No argument can be null.
        /// </exception>
        public override IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JObject entry)
        {
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
                result.AddRange(entry.Value<JArray>(FactionsPropertyName)
                                     .Where(e => ((JObject)e).Value<string>("Faction") == supportedMinorFaction)
                                     .Select(e => new RedeemVoucherSummaryEntry(GetTimeStamp(entry), entry.Value<string>("Type"), e.Value<int>("Amount"))));

                // TODO: Claiming bounties against the supported minor faction
            }
            else
            {
                if(entry.Value<string>("Faction") == supportedMinorFaction)
                {
                    result.Add(new RedeemVoucherSummaryEntry(GetTimeStamp(entry), entry.Value<string>("Type"), entry.Value<int>("Amount")));
                }

                // TODO: Claiming vouchers against the supported minor faction
            }

            return result;
        }
    }
}
