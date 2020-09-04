using EDMissionSummary.JournalEntryProcessors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDMissionSummary.SummaryEntries;

namespace EDMissionSummary
{
    public class Summarizer
    {
        public readonly string FactionEffectsSectionName = "FactionEffects";
        protected internal Dictionary<string, JournalEntryProcessor> _journalEntryProcessors;

        public Summarizer()
        {
            _journalEntryProcessors = new Dictionary<string, JournalEntryProcessor>
            {
                {  FsdJumpEntryProcessor.EventName, new FsdJumpEntryProcessor() },
                {  DockedEntryProcessor.EventName, new DockedEntryProcessor() },
                {  LocationEntryProcessor.EventName, new LocationEntryProcessor() },
                {  MissionAcceptedEntryProcessor.EventName, new MissionAcceptedEntryProcessor() },
                {  MissionCompletedEntryProcessor.EventName, new MissionCompletedEntryProcessor() },
                {  RedeemVoucherEntryProcessor.EventName, new RedeemVoucherEntryProcessor() }
            };
        }

        /// <summary>
        /// The <see cref="JournalEntryProcessor"/> types used to process journal entries.
        /// </summary>
        public IReadOnlyDictionary<string, JournalEntryProcessor> JournalEntryProcessors => _journalEntryProcessors;

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
            if (_journalEntryProcessors.TryGetValue(entry.Value<string>("event"), out JournalEntryProcessor journalEntryProcessor))
            {
                result = journalEntryProcessor.Process(pilotState, galaxyState, supportedMinorFaction, entry);
            }
            return result;
        }
    }
}
