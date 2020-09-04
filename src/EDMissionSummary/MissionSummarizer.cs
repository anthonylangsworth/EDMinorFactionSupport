using EDMissionSummary.JournalEntryProcessors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDMissionSummary.SummaryEntries;

namespace EDMissionSummary
{
    public class MissionSummarizer
    {
        public readonly string FactionEffectsSectionName = "FactionEffects";
        private Dictionary<string, JournalEntryProcessor> journalEntryProcessors;

        public MissionSummarizer()
        {
            journalEntryProcessors = new Dictionary<string, JournalEntryProcessor>
            {
                {  DockedEntryProcessor.EventName, new DockedEntryProcessor() },
                {  LocationEntryProcessor.EventName, new LocationEntryProcessor() },
                {  MissionAcceptedEntryProcessor.EventName, new MissionAcceptedEntryProcessor() },
                {  MissionCompletedEntryProcessor.EventName, new MissionCompletedEntryProcessor() },
                {  RedeemVoucherEntryProcessor.EventName, new RedeemVoucherEntryProcessor() }
            };
        }

        public IEnumerable<SummaryEntry> Convert(PilotState pilotState, GalaxyState galaxyState, string supportedMinorFaction, JObject entry)
        {
            if (pilotState is null)
            {
                throw new ArgumentNullException(nameof(pilotState));
            }
            if (supportedMinorFaction is null)
            {
                throw new ArgumentNullException(nameof(supportedMinorFaction));
            }
            if (entry == null)
            {
                throw new NullReferenceException(nameof(entry));
            }

            IEnumerable<SummaryEntry> result = Enumerable.Empty<SummaryEntry>();
            if (journalEntryProcessors.TryGetValue(entry.Value<string>("event"), out JournalEntryProcessor journalEntryProcessor))
            {
                result = journalEntryProcessor.Process(pilotState, galaxyState, supportedMinorFaction, entry);
            }
            return result;
        }
    }
}
