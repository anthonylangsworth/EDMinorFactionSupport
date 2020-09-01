using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDMissionSummary.SummaryEntries;

namespace EDMissionSummary.JournalEntryProcessors
{
    public class MissionCompletedEventProcessor : JournalEntryProcessor
    {
        public static readonly string EventName = "MissionCompleted";
        public static readonly string FactionEffectsSectionName = "FactionEffects";

        public MissionCompletedEventProcessor()
        {
            // Do nothing
        }

        public override SummaryEntry Process(PilotState pilotState, SupportedFaction supportedFaction, JObject entry)
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

            FactionSupportResult supportResult = SupportsFaction(entry, supportedFaction.Name);
            string influence = GetInfluence(entry);

            return new MissionSummaryEntry(
                    "", // influenceSection.Value<string>("SystemAddress"),
                    entry.Value<string>("DestinationSystem"),
                    supportResult == FactionSupportResult.Support,
                    influence);
        }

        /// <summary>
        /// Does this mission help the supported faction?
        /// </summary>
        /// <param name="entry">
        /// The JObject representing the journal entry to check. This cannot be null.
        /// </param>
        /// <param name="supportedFactionName">
        /// The name of the supported faction. Cannot be null, empty or whitespace.
        /// </param>
        /// <returns>
        /// True if it helps the supported faction, false if it has 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entry"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="supportedFactionName"/> cannot be null, empty or whitespace.
        /// </exception>
        protected internal static FactionSupportResult SupportsFaction(JObject entry, string supportedFactionName)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            if (string.IsNullOrWhiteSpace(supportedFactionName))
            {
                throw new ArgumentException($"'{nameof(supportedFactionName)}' cannot be null or whitespace", nameof(supportedFactionName));
            }

            FactionSupportResult result = FactionSupportResult.None;

            if (entry.Value<JArray>(FactionEffectsSectionName)
                     .Any(fe => fe.Value<string>("Faction") == supportedFactionName))
            {
                result = FactionSupportResult.Support;
            }
            else
            {
                // TODO: Check systems involved to determine whether the supported faction is present.
                // If so, it is undermining. If not, it has no effect.

                result = FactionSupportResult.Undermine;
            }

            return result;
        }


        /// <summary>
        /// Get the influence increase. Note that this assumes the influence increase for all parties is identical.
        /// </summary>
        /// <param name="entry">
        /// The JObject representing the journal entry to check. This cannot be null.
        /// </param>
        /// <returns>
        /// The influence increase, expressed as a number of "+" (plus) characters.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entry"/> cannot be null.
        /// </exception>
        protected internal static string GetInfluence(JObject entry)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return entry.Value<JArray>(FactionEffectsSectionName)
                        .SelectMany(e => e.Value<JArray>("Influence"))
                        .FirstOrDefault(e => e.Any())
                        .Value<string>("Influence");

            //return entry.Value<JArray>(FactionEffectsSectionName)
            //            .FirstOrDefault(fe => ((JObject)fe).Value<JArray>("Influence").Any())
            //            .Value<string>("Influence");
        }
    }
}
