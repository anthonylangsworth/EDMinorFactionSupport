using EDMissionSummary.JournalEntryProcessors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDMissionSummary.SummaryEntries;

namespace EDMissionSummary
{
    public class SquadronMissionSummarizer
    {
        public readonly string FactionEffectsSectionName = "FactionEffects";
        private Dictionary<string, JournalEntryProcessor> journalEntryProcessors;

        public SquadronMissionSummarizer()
        {
            journalEntryProcessors = new Dictionary<string, JournalEntryProcessor>
            {
                {  MissionCompletedEventProcessor.EventName, new MissionCompletedEventProcessor() }
            };
        }

        public SquadronSummaryEntry Convert(PilotState pilotState, SupportedFaction supportedFaction, JObject entry)
        {
            if (pilotState is null)
            {
                throw new ArgumentNullException(nameof(pilotState));
            }
            if (supportedFaction is null)
            {
                throw new ArgumentNullException(nameof(supportedFaction));
            }
            if (entry == null)
            {
                throw new NullReferenceException(nameof(entry));
            }

            JournalEntryProcessor journalEntryProcessor = journalEntryProcessors[entry.Value<string>("event")];
            return journalEntryProcessor?.Process(pilotState, supportedFaction, entry);
        }
    }
}
