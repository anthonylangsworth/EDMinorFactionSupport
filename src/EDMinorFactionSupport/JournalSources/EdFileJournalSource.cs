﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDMinorFactionSupport.JournalSources
{
    public class EdFileJournalSource : JournalSource
    {
        public EdFileJournalSource(DateTime date)
        {
            Date = date.Date;
        }

        public DateTime Date
        {
            get;
        }

        public override IEnumerable<string> Entries
        {
            get
            {
                DirectoryInfo journalFolder = new DirectoryInfo(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        "Saved Games\\Frontier Developments\\Elite Dangerous\\"));
                IEnumerable<FileInfo> journalFiles = journalFolder.GetFiles("Journal.*.log")
                                                                  .Where(f => Date == DateTime.MinValue || f.LastWriteTime.Date == Date)
                                                                  .OrderBy(f => f.LastWriteTime);
                return journalFiles.Select(jf => jf.FullName)
                                   .SelectMany(fileName => new FileJournalSource(fileName).Entries);
            }
        }
    }
}
