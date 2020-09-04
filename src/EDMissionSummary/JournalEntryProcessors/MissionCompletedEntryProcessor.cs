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
            if(!pilotState.Missions.ContainsKey(entry.Value<long>("MissionID")))
            {
                throw new InvalidOperationException($"Mission { entry.Value<long>("MissionID") } is unknown");
            }

            List<SummaryEntry> result = new List<SummaryEntry>();
            Mission mission = pilotState.Missions[entry.Value<long>("MissionID")];

            foreach (JObject influenceObject in entry.Value<JArray>(FactionEffectsSectionName)
                                                    .SelectMany(e => e.Value<JArray>("Influence").Children<JObject>()))
            {
                string influenceFaction = influenceObject.Parent.Parent.Parent.Value<string>("Faction");
                string entryFaction = entry.Value<string>("Faction");
                string faction = string.IsNullOrWhiteSpace(influenceFaction) ? entryFaction : influenceFaction;
                string entryTargetFaction = entry.Value<string>("TargetFaction");

                if (influenceObject.HasValues)
                {
                    StarSystem system = galaxyState.Systems[influenceObject.Value<long>("SystemAddress")];
                    string influence = influenceObject.Value<string>("Influence");
                    bool influenceIncrease = influenceObject.Value<string>("Trend") == "UpGood";
                    DateTime timeStamp = GetTimeStamp(entry);

                    if (faction == supportedMinorFaction)
                    {
                        result.Add(new MissionSummaryEntry(
                            timeStamp,
                            mission.Name,
                            system.Name,
                            influenceIncrease,
                            influence));
                    }
                    else if (system.MinorFactions.Contains(faction))
                    {
                        result.Add(new MissionSummaryEntry(
                            timeStamp,
                            mission.Name,
                            system.Name,
                            !influenceIncrease,
                            influence));
                    }
                }
            }

            return result;
        }
    }
}
