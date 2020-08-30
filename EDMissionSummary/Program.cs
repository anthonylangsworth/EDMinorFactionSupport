using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace EDMissionSummary
{
    class Program
    {
        static void Main(string[] args)
        {
            Journal journal = new Journal(args[0]);
            JournalEntryParser journalEntryParser = new JournalEntryParser();

            foreach (JObject entry in journal.Entries.Select(journalEntryParser.Parse))
            {
                if(entry.Value<string>("event") == "MissionCompleted")
            }
        }
    }
}
