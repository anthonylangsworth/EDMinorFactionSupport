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
        public static readonly string FactionPropertyName = "Faction";
        public static readonly string FactionsPropertyName = "Factions";
        public static readonly string AmountPropertyName = "Amount";

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

            StarSystem starSystem = galaxyState.Systems[pilotState.LastDockedStation.SystemAddress];
            Station station = pilotState.LastDockedStation;

            List<SummaryEntry> result = new List<SummaryEntry>();
            if (entry.Value<string>(TypePropertyName) == BountyValue)
            {
                var categorizedEntries = entry.Value<JArray>(FactionsPropertyName)
                                     .Select(e => (JObject)e)
                                     .Select(e => new { Entry = e, FactionSupport = GetFactionInfluence(supportedMinorFaction, e.Value<string>(FactionPropertyName), station.ControllingMinorFaction, station.MinorFactions) });
                result.AddRange(categorizedEntries
                                     .Where(e => e.FactionSupport == FactionInfluence.Increase)
                                     .Select(e => new RedeemVoucherSummaryEntry(GetTimeStamp(entry), starSystem?.Name, true, entry.Value<string>(TypePropertyName), e.Entry.Value<int>(AmountPropertyName))));
                result.AddRange(categorizedEntries
                                     .Where(e => e.FactionSupport == FactionInfluence.Decrease)
                                     .Select(e => new RedeemVoucherSummaryEntry(GetTimeStamp(entry), starSystem?.Name, false, entry.Value<string>(TypePropertyName), e.Entry.Value<int>(AmountPropertyName))));
            }
            else
            {
                if(entry.Value<string>(FactionPropertyName) == supportedMinorFaction)
                {
                    result.Add(new RedeemVoucherSummaryEntry(GetTimeStamp(entry), starSystem?.Name, true, entry.Value<string>(TypePropertyName), entry.Value<int>(AmountPropertyName)));
                }

                // TODO: Claiming vouchers against the supported minor faction
            }

            return result;
        }

        /// <summary>
        /// Determine whether the journal entry affects the supported minor faction's influence in the system.
        /// </summary>
        /// <param name="supportedMinorFaction">
        /// The name of the supported minor faction. This cannot be null.
        /// </param>
        /// <param name="entryFaction">
        /// The faction affeted by the journal entry. For example, it was a mission supplier or the payer of combat bonds.  This cannot be null.
        /// </param>
        /// <param name="stationControllingFaction">
        /// The faction controlling the station.  This cannot be null.
        /// </param>
        /// <param name="stationMinorFactions">
        /// The minor factions present but not controlling the station.  This cannot be null.
        /// </param>
        /// <returns>
        /// A <see cref="FactionInfluence"/> indicating what affect it had on minor faction influence.
        /// </returns>
        public static FactionInfluence GetFactionInfluence(string supportedMinorFaction, string entryFaction, string stationControllingFaction, IEnumerable<string> stationMinorFactions)
        {
            if (supportedMinorFaction is null)
            {
                throw new ArgumentNullException(nameof(supportedMinorFaction));
            }
            if (string.IsNullOrEmpty(entryFaction))
            {
                throw new ArgumentException($"'{nameof(entryFaction)}' cannot be null or empty", nameof(entryFaction));
            }
            if (string.IsNullOrEmpty(stationControllingFaction))
            {
                throw new ArgumentException($"'{nameof(stationControllingFaction)}' cannot be null or empty", nameof(stationControllingFaction));
            }
            if (stationMinorFactions is null)
            {
                throw new ArgumentNullException(nameof(stationMinorFactions));
            }

            FactionInfluence result = FactionInfluence.None;
            if (supportedMinorFaction == entryFaction)
            {
                result = FactionInfluence.Increase;
            }
            else
            {
                IEnumerable<string> factions = stationMinorFactions.Concat(new[] { stationControllingFaction });
                if(factions.Contains(supportedMinorFaction) && factions.Contains(entryFaction))
                {
                    result = FactionInfluence.Decrease;
                }
            }
            return result;
        }
    }
}
