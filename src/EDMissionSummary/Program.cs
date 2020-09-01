using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EDMissionSummary.JournalEntryProcessors;
using EDMissionSummary.SummaryEntries;
using EDMissionSummary.JournalSources;
using System.Text;

namespace EDMissionSummary
{
    class Program
    {
        static void Main(string[] args)
        {
            // string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830134805.01.log";
            // string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830102216.01.log";
            // string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830212509.01.log";
            // string fileName = @"TestMissions.log";

            //try
            //{
                JournalSource journal = new EdFileJournalSource(DateTime.Now.AddDays(-1)); // new FileJournalSource(fileName);
                JournalEntryParser journalEntryParser = new JournalEntryParser();
                MissionSummarizer missionSummarizer = new MissionSummarizer();
                PilotState pilotState = new PilotState();
                SupportedFaction supportedFaction = new SupportedFaction("EDA Kunti League", new string[0], new string[0]);

            Console.Out.WriteLine(
                journal.Entries
                       .Select(journalEntryParser.Parse)
                       .Select(entry => missionSummarizer.Convert(pilotState, supportedFaction, entry))
                       .Where(e => e != null)
                       .Aggregate(new StringBuilder(), (sb, se) => sb.AppendLine(se.ToString())));
            //}
            //catch(Exception ex)
            //{
            //    Console.Error.Write(ex.ToString());
            //}
        }
    }
}
