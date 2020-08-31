using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace EDMissionSummary.JournalSources
{
    public class FileJournalSource: JournalSource
    {
        public FileJournalSource(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Journal file not found", fileName);
            }

            FileName = fileName;
        }

        public string FileName
        {
            get;
        }

        public override IEnumerable<string> Entries
        {
            get
            {
                return File.ReadAllLines(FileName);
            }
        }
    }
}
