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
                {  MissionCompletedEventProcessor.EventName, new MissionCompletedEventProcessor() },
                {  RedeemVoucherEventProcessor.EventName, new RedeemVoucherEventProcessor() }
            };
        }

        public SummaryEntry Convert(PilotState pilotState, SupportedFaction supportedFaction, JObject entry)
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

            JournalEntryProcessor journalEntryProcessor;
            journalEntryProcessors.TryGetValue(entry.Value<string>("event"), out journalEntryProcessor);
            return journalEntryProcessor?.Process(pilotState, supportedFaction, entry);
        }
    }
}
