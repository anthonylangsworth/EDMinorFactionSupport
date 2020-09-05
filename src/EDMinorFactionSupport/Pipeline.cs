using EDMinorFactionSupport.JournalSources;
using EDMinorFactionSupport.SummaryEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMinorFactionSupport
{
    public class Pipeline
    {
        public Pipeline(JournalSource journalSource, JournalEntryParser journalEntryParser, Summarizer summarizer)
        {
            JournalSource = journalSource ?? throw new ArgumentNullException(nameof(journalSource));
            JournalEntryParser = journalEntryParser ?? throw new ArgumentNullException(nameof(journalEntryParser));
            Summarizer = summarizer ?? throw new ArgumentNullException(nameof(summarizer));
        }

        public JournalSource JournalSource { get; }
        public JournalEntryParser JournalEntryParser { get; }
        public Summarizer Summarizer { get; }

        public IEnumerable<SummaryEntry> Run(string supportedMinorFaction)
        {
            if (string.IsNullOrWhiteSpace(supportedMinorFaction))
            {
                throw new ArgumentException($"'{nameof(supportedMinorFaction)}' cannot be null or whitespace", nameof(supportedMinorFaction));
            }

            PilotState pilotState = new PilotState();
            GalaxyState galaxyState = new GalaxyState();

            return JournalSource.Entries
                                .Select(JournalEntryParser.Parse)
                                .SelectMany(entry => Summarizer.Convert(pilotState, galaxyState, supportedMinorFaction, entry));
        }
    }
}
