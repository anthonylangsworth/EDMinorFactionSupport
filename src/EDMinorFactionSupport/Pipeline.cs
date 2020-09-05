using EDMinorFactionSupport.JournalSources;
using EDMinorFactionSupport.OutputFormatters;
using EDMinorFactionSupport.SummaryEntries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDMinorFactionSupport
{
    public class Pipeline
    {
        public Pipeline(JournalSource journalSource, JournalEntryParser journalEntryParser, Summarizer summarizer, OutputFormatter outputFormatter)
        {
            JournalSource = journalSource ?? throw new ArgumentNullException(nameof(journalSource));
            JournalEntryParser = journalEntryParser ?? throw new ArgumentNullException(nameof(journalEntryParser));
            Summarizer = summarizer ?? throw new ArgumentNullException(nameof(summarizer));
            OutputFormatter = outputFormatter ?? throw new ArgumentNullException(nameof(outputFormatter));
        }

        public JournalSource JournalSource { get; }
        public JournalEntryParser JournalEntryParser { get; }
        public Summarizer Summarizer { get; }
        public OutputFormatter OutputFormatter { get; }

        public void Run(string supportedMinorFaction, TextWriter output)
        {
            if (string.IsNullOrWhiteSpace(supportedMinorFaction))
            {
                throw new ArgumentException($"'{nameof(supportedMinorFaction)}' cannot be null or whitespace", nameof(supportedMinorFaction));
            }
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            PilotState pilotState = new PilotState();
            GalaxyState galaxyState = new GalaxyState();

            OutputFormatter.Format(
                JournalSource.Entries
                             .Select(JournalEntryParser.Parse)
                             .SelectMany(entry => Summarizer.Convert(pilotState, galaxyState, supportedMinorFaction, entry)),
                output);
        }
    }
}
