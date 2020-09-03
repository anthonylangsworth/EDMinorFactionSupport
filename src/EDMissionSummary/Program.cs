﻿using System;
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
                JournalSource journal = new EdFileJournalSource(DateTime.Now); // new FileJournalSource(fileName);
                JournalEntryParser journalEntryParser = new JournalEntryParser();
                MissionSummarizer missionSummarizer = new MissionSummarizer();
                PilotState pilotState = new PilotState();
                GalaxyState galaxyState = new GalaxyState();
                string supportedFaction = "EDA Kunti League";

            //IEnumerable<SummaryEntry> missionSummaryEntries = 
            //    journal.Entries
            //           .Select(journalEntryParser.Parse)
            //           .Select(entry => missionSummarizer.Convert(pilotState, supportedFaction, entry))
            //           .Where(e => e != null);

            Console.Out.WriteLine(
                journal.Entries
                       .Select(journalEntryParser.Parse)
                       .SelectMany(entry => missionSummarizer.Convert(pilotState, galaxyState, supportedFaction, entry))
                       .Aggregate(new StringBuilder(), (sb, se) => sb.AppendLine(se.ToString())));
            //}
            //catch(Exception ex)
            //{
            //    Console.Error.Write(ex.ToString());
            //}
        }

        //static void DisplayMissions(IEnumerable<MissionSummaryEntry> missionSummaryEntries)
        //{
        //    missionSummaryEntries.GroupBy(mse => mse.SourceSystemId);
        //}
    }
}
