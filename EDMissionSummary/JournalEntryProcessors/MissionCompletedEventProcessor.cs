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

            SquadronSummaryMissionEntry result = null;
            JArray factionEffects = entry.Value<JArray>(FactionEffectsSectionName);
            FactionSupportResult supportResult;

            // TODO: Find the sections with influence.
            // Of those sections, find the one that matches the supported faction.
            JObject factionEffect = factionEffects.FirstOrDefault(
                fe => fe.Value<string>("Faction") == supportedFaction.Name
                   && fe.Value<JArray>("Influence").Any()) as JObject;
            if (factionEffect != null)
            {
                supportResult = FactionSupportResult.Support;
            }
            else
            {
                // If it is not in the list, take the first entry that has an influence section to determine the 
                //
                // Workaround: Assume the influence gain is the same for all parties and use the first entry
                // with a supplied influence gain.
                factionEffect = factionEffects.FirstOrDefault(fe => ((JObject)fe).Value<JArray>("Influence").Any()).Value<JObject>();

                // It is only working 
                supportResult = FactionSupportResult.Undermine;
            }

            if (factionEffect != null && supportResult != FactionSupportResult.None)
            {
                JToken influenceSection = factionEffect["Influence"].FirstOrDefault();
                string influencePluses = influenceSection.Value<string>("Influence");

                result = new SquadronSummaryMissionEntry(
                    influenceSection.Value<string>("SystemAddress"),
                    entry.Value<string>("DestinationSystem"),
                    supportResult == FactionSupportResult.Support,
                    influenceSection.Value<string>("Influence"));
            }

            return result;
        }

        /// <summary>
        /// Does this mission help the supported faction?
        /// </summary>
        /// <param name="entry">
        /// The JObject representing the journal entry to check. This cannot be null.
        /// </param>
        /// <param name="supportedFaction">
        /// The name of the supported faction. Cannot be null, empty or whitespace.
        /// </param>
        /// <returns>
        /// True if it helps the supported faction, false if it has 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entry"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="supportedFaction"/> cannot be null, empty or whitespace.
        /// </exception>
        protected  static FactionSupportResult SupportsFaction(JObject entry, string supportedFaction)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            if (string.IsNullOrWhiteSpace(supportedFaction))
            {
                throw new ArgumentException($"'{nameof(supportedFaction)}' cannot be null or whitespace", nameof(supportedFaction));
            }

            FactionSupportResult result = FactionSupportResult.None;

            if (entry.Value<JArray>("FactionEffectsSectionName")
                     .Any(fe => fe.Value<string>("Faction") == supportedFaction))
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
        private string GetInfluence(JObject entry)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return entry.Value<JArray>("FactionEffectsSectionName")
                        .FirstOrDefault(fe => ((JObject)fe)
                        .Value<JArray>("Influence").Any())
                        .Value<JObject>()
                        .Value<string>("Influence");
        }
    }
}
