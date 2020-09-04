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

            List<SummaryEntry> result = new List<SummaryEntry>();
            string influence = GetInfluence(entry);
            JObject factionObject = (JObject)entry.Value<JArray>(FactionEffectsSectionName)
                                                  .FirstOrDefault(fe => fe.Value<string>("Faction") == supportedMinorFaction);
            if (factionObject != null)
            {
                result.AddRange(
                    factionObject.Value<JArray>("Influence")
                                 .Select(e => new MissionSummaryEntry(
                                    GetTimeStamp(entry),
                                    galaxyState.GetSystemName(e.Value<long>("SystemAddress")),
                                    true,
                                    influence)));
            }

            return result;
        }

        /// <summary>
        /// Does this mission help the supported faction?
        /// </summary>
        /// <param name="entry">
        /// The JObject representing the journal entry to check. This cannot be null.
        /// </param>
        /// <param name="supportedMinorFaction">
        /// The name of the supported faction. Cannot be null, empty or whitespace.
        /// </param>
        /// <returns>
        /// True if it helps the supported faction, false if it has 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entry"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="supportedMinorFaction"/> cannot be null, empty or whitespace.
        /// </exception>
        protected internal static FactionInfluence SupportsFaction(JObject entry, string supportedMinorFaction)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            if (string.IsNullOrWhiteSpace(supportedMinorFaction))
            {
                throw new ArgumentException($"'{nameof(supportedMinorFaction)}' cannot be null or whitespace", nameof(supportedMinorFaction));
            }

            FactionInfluence result = FactionInfluence.None;

            if (entry.Value<JArray>(FactionEffectsSectionName)
                     .Any(fe => fe.Value<string>("Faction") == supportedMinorFaction))
            {
                result = FactionInfluence.Increase;
            }
            else
            {
                // TODO: Check systems involved to determine whether the supported faction is present.
                // If so, it is undermining. If not, it has no effect.

                result = FactionInfluence.Decrease;
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
