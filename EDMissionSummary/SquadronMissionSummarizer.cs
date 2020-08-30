using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMissionSummary
{
    public class SquadronMissionSummarizer
    {
        public readonly string MissionCompletedEvent = "MissionCompleted";
        public readonly string FactionEffectsSectionName = "FactionEffects";

        public SquadronMissionSummarizer(string supportedFaction)
        {
            if(string.IsNullOrWhiteSpace(supportedFaction))
            {
                throw new ArgumentException("Cannot be null or whitespace", nameof(supportedFaction));
            }

            SupportedFaction = supportedFaction;
        }

        public string SupportedFaction
        {
            get;
        }


        public SquadronSummaryEntry Convert(JObject entry)
        {
            // ISet<Mission> missions for state machine?

            if (entry == null)
            {
                throw new NullReferenceException(nameof(entry));
            }

            SquadronSummaryEntry result = null;
            if (entry.Value<string>("event") == MissionCompletedEvent)
            {
                JArray factionEffects = entry.Property(FactionEffectsSectionName).Value<JArray>();

                // Assume the supported faction is the destination faction, meaning their influence gain is 
                // included in the JSON result.
                JObject factionEffect = factionEffects.FirstOrDefault(fe => fe.Value<string>("Faction") == SupportedFaction) as JObject;
                if (factionEffect == null)
                {
                    // If not, the supported faction is the source faction, meaing their influence gain is not
                    // included in the JSON entry. Instead, it is stored in the "NissionAccepted" entry.
                    //
                    // Workaround: Assume the influence gain is the same for all parties and use the first entry
                    // with a supplied influence gain.
                    factionEffect = factionEffects.FirstOrDefault(fe => ((JObject)fe).Property("Influence").Value<JArray>().Any()).Value<JObject>();
                }

                JToken influenceSection = factionEffect.Property("Influence").Value<JArray>().FirstOrDefault();
                string influencePluses = influenceSection.Value<string>("Influence");

                result = new SquadronSummaryMissionEntry(
                    influenceSection.Value<string>("SystemAddress"),
                    entry.Value<string>("DestinationSystem"),
                    true,
                    influenceSection.Value<string>("Influence"));

                //}
                //else
                //{
                //    // The supported faction is the source faction, meaing their influence gain is not
                //    // included in the JSON entry. Instead, it is stored in the "NissionAccepted" entry.
                //    //
                //    // Workaround: Assume the influence gain is the same for all parties and use the first entry
                //    // with a supplied influence gain.

                //    JObject firstFactionEffect = factionEffects.FirstOrDefault(fe => ((JObject) fe).Property("Influence").Value<JArray>().Any()).Value<JObject>();
                //    if (firstFactionEffect != null)
                //    {
                //        string influencePluses = influenceSection.Value<string>("Influence");

                //        result = new SquadronSummaryMissionEntry(
                //            influenceSection.Value<string>("SystemAddress"),
                //            entry.Value<string>("DestinationSystem"),
                //            true,
                //            influenceSection.Value<string>("Influence"));
                //    }
                //}
            }

            return result;
        }
    }
}
