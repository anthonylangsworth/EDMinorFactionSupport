using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EDMinorFactionSupport.JournalSources
{
    /// <summary>
    /// Read journal entries from a string, usually used for testing.
    /// </summary>
    internal class StringJournalSource : JournalSource
    {
        /// <summary>
        /// Create a new <see cref="StringJournalSource"/>.
        /// </summary>
        /// <param name="journalText"></param>
        public StringJournalSource(string journalText)
        {
            if (string.IsNullOrWhiteSpace(journalText))
            {
                throw new ArgumentException($"'{nameof(journalText)}' cannot be null or whitespace", nameof(journalText));
            }

            JournalText = journalText;
        }

        /// <summary>
        /// The text.
        /// </summary>
        public string JournalText
        {
            get;
        }

        /// <summary>
        /// The entries.
        /// </summary>
        public override IEnumerable<string> Entries
        {
            get
            {
                using (StringReader stringReader = new StringReader(JournalText))
                {
                    yield return stringReader.ReadLine();
                }
            }
        }
    }
}
