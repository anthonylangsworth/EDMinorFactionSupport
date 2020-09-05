using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace EDMinorFactionSupport.JournalSources
{
    /// <summary>
    /// Read Elite: Dangerous journal entries from a specific file.
    /// </summary>
    public class FileJournalSource: JournalSource
    {
        /// <summary>
        /// Create a new <see cref="FileJournalSource"/>.
        /// </summary>
        /// <param name="fileName">
        /// The path to the file. This file must exist.
        /// </param>
        /// <exception cref="FileNotFoundException">
        /// The file <paramref name="fileName"/> does not exist.
        /// </exception>
        public FileJournalSource(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Journal file not found", fileName);
            }

            FileName = fileName;
        }

        /// <summary>
        /// The file name.
        /// </summary>
        public string FileName
        {
            get;
        }

        /// <summary>
        /// The Elite: Dangerous journal entries.
        /// </summary>
        public override IEnumerable<string> Entries
        {
            get
            {
                return File.ReadAllLines(FileName);
            }
        }
    }
}
