using EDMinorFactionSupport.SummaryEntries;
using NSW.EliteDangerous.API;
using NSW.EliteDangerous.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMinorFactionSupport.JournalEntryProcessors
{
    public class RedeemVoucherEntryProcessor : JournalEventProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        public override string EventName => "RedeemVoucher";

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
        public override IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JournalEvent journalEvent)
        {
            if (supportedMinorFaction is null)
            {
                throw new ArgumentNullException(nameof(supportedMinorFaction));
            }
            if (journalEvent is null)
            {
                throw new ArgumentNullException(nameof(journalEvent));
            }
            if(pilotState.LastDockedStation == null)
            {
                throw new InvalidOperationException("Has not docked at a station");
            }
            if (!galaxyState.Systems.ContainsKey(pilotState.LastDockedStation.SystemAddress))
            {
                throw new InvalidOperationException($"System address { pilotState.LastDockedStation.SystemAddress } not found");
            }

            RedeemVoucherEvent redeemVoucherEvent = (RedeemVoucherEvent)journalEvent;
            string systemName = galaxyState.GetSystemName(pilotState.LastDockedStation.SystemAddress);
            Station station = pilotState.LastDockedStation;

            List<SummaryEntry> result = new List<SummaryEntry>();
            if (redeemVoucherEvent.Type == VoucherType.Bounty)
            {
                var categorizedEntries = redeemVoucherEvent.Factions
                                                           .Select(f => new { Entry = f, FactionInfluence = GetFactionInfluence(supportedMinorFaction, f.Faction, station.ControllingMinorFaction, galaxyState.Systems[pilotState.LastDockedStation.SystemAddress].MinorFactions) });
                result.AddRange(categorizedEntries
                                     .Where(e => e.FactionInfluence == FactionInfluence.Increase)
                                     .Select(e => new RedeemVoucherSummaryEntry(redeemVoucherEvent.Timestamp, systemName, true, redeemVoucherEvent.Type, e.Entry.Amount)));
                result.AddRange(categorizedEntries
                                     .Where(e => e.FactionInfluence == FactionInfluence.Decrease)
                                     .Select(e => new RedeemVoucherSummaryEntry(redeemVoucherEvent.Timestamp, systemName, false, redeemVoucherEvent.Type, e.Entry.Amount)));
            }
            else
            {
                FactionInfluence factionInfluence = GetFactionInfluence(supportedMinorFaction, redeemVoucherEvent.Faction, station.ControllingMinorFaction, galaxyState.Systems[pilotState.LastDockedStation.SystemAddress].MinorFactions);
                if (factionInfluence != FactionInfluence.None)
                {
                    result.Add(new RedeemVoucherSummaryEntry(redeemVoucherEvent.Timestamp, systemName, factionInfluence == FactionInfluence.Increase, redeemVoucherEvent.Type, redeemVoucherEvent.Amount));
                }
            }

            return result;
        }

        /// <summary>
        /// Determine whether the journal entry affects the supported minor faction's influence in the system.
        /// </summary>
        /// <param name="supportedMinorFaction">
        /// The name of the supported minor faction. This cannot be null.
        /// </param>
        /// <param name="journalEntryFaction">
        /// The faction affeted by the journal entry. For example, it was a mission supplier or the payer of combat bonds. If null or empty, assumed to be the station controlling faction.
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
        public static FactionInfluence GetFactionInfluence(string supportedMinorFaction, string journalEntryFaction, string stationControllingFaction, IEnumerable<string> stationMinorFactions)
        {
            if (supportedMinorFaction is null)
            {
                throw new ArgumentNullException(nameof(supportedMinorFaction));
            }
            if (string.IsNullOrEmpty(journalEntryFaction))
            {
                journalEntryFaction = stationControllingFaction;
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
            if (supportedMinorFaction == journalEntryFaction)
            {
                result = FactionInfluence.Increase;
            }
            else
            {
                IEnumerable<string> factions = stationMinorFactions.Concat(new[] { stationControllingFaction });
                if(factions.Contains(supportedMinorFaction) && factions.Contains(journalEntryFaction))
                {
                    result = FactionInfluence.Decrease;
                }
            }
            return result;
        }
    }
}
