using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDMissionSummary.SummaryEntries;

namespace EDMissionSummary.JournalEntryProcessors
{
    public class MissionCompletedEntryProcessor : JournalEntryProcessor
    {
        public static readonly string EventName = "MissionCompleted";
        public static readonly string FactionEffectsSectionName = "FactionEffects";

        public override IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction,  JObject entry)
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

            FactionSupport supportResult = SupportsFaction(entry, supportedMinorFaction);
            string influence = GetInfluence(entry);

            List<SummaryEntry> result = new List<SummaryEntry>();

            // TODO: FIX!!!!
            //result.Add(new MissionSummaryEntry(
            //        "", // influenceSection.Value<string>("SystemAddress"),
            //        entry.Value<string>("DestinationSystem"),
            //        supportResult == FactionSupport.Support,
            //        influence));

            return result;
        }

        /// <summary>
        /// Does this mission help the supported faction?
        /// </summary>
        /// <param name="entry">
        /// The JObject representing the journal entry to check. This cannot be null.
        /// </param>
        /// <param name="supportedMinorFactionName">
        /// The name of the supported faction. Cannot be null, empty or whitespace.
        /// </param>
        /// <returns>
        /// True if it helps the supported faction, false if it has 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entry"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="supportedMinorFactionName"/> cannot be null, empty or whitespace.
        /// </exception>
        protected internal static FactionSupport SupportsFaction(JObject entry, string supportedMinorFactionName)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            if (string.IsNullOrWhiteSpace(supportedMinorFactionName))
            {
                throw new ArgumentException($"'{nameof(supportedMinorFactionName)}' cannot be null or whitespace", nameof(supportedMinorFactionName));
            }

            FactionSupport result = FactionSupport.None;

            if (entry.Value<JArray>(FactionEffectsSectionName)
                     .Any(fe => fe.Value<string>("Faction") == supportedMinorFactionName))
            {
                result = FactionSupport.Support;
            }
            else
            {
                // TODO: Check systems involved to determine whether the supported faction is present.
                // If so, it is undermining. If not, it has no effect.

                result = FactionSupport.Undermine;
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
