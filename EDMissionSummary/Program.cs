using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace EDMissionSummary
{
    class Program
    {
        static void Main(string[] args)
        {
            // string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830134805.01.log";
            string fileName = @"Missions.log";
            string supportedSquadron = "EDA Kunti League";

            try
            {
                Journal journal = new Journal(fileName);// args[0]
                JournalEntryParser journalEntryParser = new JournalEntryParser();
                SquadronMissionSummarizer squadronMissionSummarizer = new SquadronMissionSummarizer(supportedSquadron);

                var output = journal.Entries
                    .Select(journalEntryParser.Parse)
                    .Select(squadronMissionSummarizer.Convert).ToList();
            }
            catch(Exception ex)
            {
                Console.Error.Write(ex.ToString());
            }
        }
    }
}
