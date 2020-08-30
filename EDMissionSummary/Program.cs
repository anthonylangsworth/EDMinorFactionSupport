using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EDMissionSummary.JournalEntryProcessors;
using EDMissionSummary.SummaryEntries;
using Newtonsoft.Json.Linq;

namespace EDMissionSummary
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830134805.01.log";
            // string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830102216.01.log";
            // string fileName = @"Missions.log";
            string supportedFactionName = "EDA Kunti League";

            //try
            //{
                Journal journal = new Journal(fileName);
                JournalEntryParser journalEntryParser = new JournalEntryParser();
                SquadronMissionSummarizer squadronMissionSummarizer = new SquadronMissionSummarizer();
                PilotState pilotState = new PilotState();
                SupportedFaction supportedFaction = new SupportedFaction(supportedFactionName, new string[0], new string[0]);

            foreach (SquadronSummaryEntry summaryEntry in journal.Entries
                    .Select(journalEntryParser.Parse)
                    .Select(entry => squadronMissionSummarizer.Convert(pilotState, supportedFaction, entry))
                    .Where(e => e != null))
                {
                    Console.Out.WriteLine(summaryEntry.ToString());
                }
            //}
            //catch(Exception ex)
            //{
            //    Console.Error.Write(ex.ToString());
            //}
        }
    }
}
