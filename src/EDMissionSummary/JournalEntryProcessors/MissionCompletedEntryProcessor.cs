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
            string missionName = pilotState.Missions.TryGetValue(entry.Value<long>("MissionID"), out Mission mission) ? mission.Name : entry.Value<string>("Name");

            foreach (JObject influenceObject in entry.Value<JArray>(FactionEffectsSectionName)
                                                    .SelectMany(e => e.Value<JArray>("Influence").Children<JObject>()))
            {
                string influenceFaction = influenceObject.Parent.Parent.Parent.Value<string>("Faction");
                string entryFaction = entry.Value<string>("Faction");
                string faction = string.IsNullOrWhiteSpace(influenceFaction) ? entryFaction : influenceFaction;
                string entryTargetFaction = entry.Value<string>("TargetFaction");

                if (influenceObject.HasValues)
                {
                    string systemName = galaxyState.Systems.TryGetValue(influenceObject.Value<long>("SystemAddress"), out StarSystem system) ? system.Name : influenceObject.Value<string>("SystemAddress");
                    string influence = influenceObject.Value<string>("Influence");
                    bool influenceIncrease = influenceObject.Value<string>("Trend") == "UpGood";
                    DateTime timeStamp = GetTimeStamp(entry);

                    if (faction == supportedMinorFaction)
                    {
                        result.Add(new MissionSummaryEntry(
                            timeStamp,
                            missionName,
                            systemName,
                            influenceIncrease,
                            influence));
                    }
                    else if (system != null && system.MinorFactions.Contains(supportedMinorFaction))
                    {
                        result.Add(new MissionSummaryEntry(
                            timeStamp,
                            missionName,
                            systemName,
                            !influenceIncrease,
                            influence));
                    }
                }
            }

            return result;
        }
    }
}
