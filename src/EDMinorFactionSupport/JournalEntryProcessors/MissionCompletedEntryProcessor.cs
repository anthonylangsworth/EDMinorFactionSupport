using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDMinorFactionSupport.SummaryEntries;
using NSW.EliteDangerous.API;
using NSW.EliteDangerous.API.Events;

namespace EDMinorFactionSupport.JournalEntryProcessors
{
    public class MissionCompletedEntryProcessor : JournalEventProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        public override string EventName => "MissionCompleted";

        public static readonly string FactionEffectsSectionName = "FactionEffects";

        public override IEnumerable<SummaryEntry> Process(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JournalEvent journalEvent)
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
            if (journalEvent is null)
            {
                throw new ArgumentNullException(nameof(journalEvent));
            }

            MissionCompletedEvent missionCompletedEvent = (MissionCompletedEvent)journalEvent;

            List<SummaryEntry> result = new List<SummaryEntry>();
            string missionName = pilotState.Missions.TryGetValue(missionCompletedEvent.MissionId, out Mission mission) ? mission.Name : missionCompletedEvent.Name;

            //foreach (Influence influence in missionCompletedEvent.FactionEffects
            //                                                     .SelectMany(fe => fe.Influence))
            //{
            //    string influenceFaction = influence.Parent.Parent.Parent.Value<string>("Faction");
            //    string entryFaction = entry.Value<string>("Faction");
            //    string faction = string.IsNullOrWhiteSpace(influenceFaction) ? entryFaction : influenceFaction;
            //    string entryTargetFaction = entry.Value<string>("TargetFaction");

            //    if (influence != null)
            //    {
            //        string systemName = galaxyState.Systems.TryGetValue(influence.Value<long>("SystemAddress"), out StarSystem system) ? system.Name : influence.Value<string>("SystemAddress");
            //        string influence = influence.Value<string>("Influence");
            //        bool influenceIncrease = influence.Value<string>("Trend") == "UpGood";
            //        DateTime timeStamp = GetTimeStamp(entry);

            //        if (faction == supportedMinorFaction)
            //        {
            //            result.Add(new MissionSummaryEntry(
            //                timeStamp,
            //                missionName,
            //                systemName,
            //                influenceIncrease,
            //                influence));
            //        }
            //        else if (system != null 
            //            && system.MinorFactions.Contains(supportedMinorFaction))
            //        {
            //            result.Add(new MissionSummaryEntry(
            //                timeStamp,
            //                missionName,
            //                systemName,
            //                !influenceIncrease,
            //                influence));
            //        }
            //    }
            //}

            return result;
        }
    }
}
