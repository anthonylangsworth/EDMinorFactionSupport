using EDMinorFactionSupport.JournalEntryProcessors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDMinorFactionSupport.SummaryEntries;

namespace EDMinorFactionSupport
{
    public class Summarizer
    {
        /// <summary>
        /// Create a new <see cref="Summarizer"/>.
        /// </summary>
        /// <param name="journalEntryProcessors"></param>
        public Summarizer(IEnumerable<JournalEntryProcessor> journalEntryProcessors)
        {
            if (journalEntryProcessors is null)
            {
                throw new ArgumentNullException(nameof(journalEntryProcessors));
            }

            JournalEntryProcessors = journalEntryProcessors.ToDictionary(jep => jep.EventName);

            // TODO: Consider injection for building this list
            //_journalEntryProcessors = new Dictionary<string, JournalEntryProcessor>
            //{
            //    {  FsdJumpEntryProcessor.EventName, new FsdJumpEntryProcessor() },
            //    {  DockedEntryProcessor.EventName, new DockedEntryProcessor() },
            //    {  LocationEntryProcessor.EventName, new LocationEntryProcessor() },
            //    {  MissionAcceptedEntryProcessor.EventName, new MissionAcceptedEntryProcessor() },
            //    {  MissionCompletedEntryProcessor.EventName, new MissionCompletedEntryProcessor() },
            //    {  RedeemVoucherEntryProcessor.EventName, new RedeemVoucherEntryProcessor() }
            //};
        }

        /// <summary>
        /// The <see cref="JournalEntryProcessor"/> types used to process journal entries.
        /// </summary>
        public IReadOnlyDictionary<string, JournalEntryProcessor> JournalEntryProcessors;

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
            if (JournalEntryProcessors.TryGetValue(entry.Value<string>("event"), out JournalEntryProcessor journalEntryProcessor))
            {
                result = journalEntryProcessor.Process(pilotState, galaxyState, supportedMinorFaction, entry);
            }
            return result;
        }
    }
}
