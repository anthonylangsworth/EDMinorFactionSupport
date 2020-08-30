using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            // string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830134805.01.log";
            // string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830102216.01.log";
            string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830212509.01.log";
            // string fileName = @"Missions.log";
            string supportedFactionName = "EDA Kunti League";

            //try
            //{
                // TODO: Move this into a derived class of Journal
                DirectoryInfo journalFolder = new DirectoryInfo(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 
                        "Saved Games\\Frontier Developments\\Elite Dangerous\\"));
                IEnumerable<FileInfo> journalFiles = journalFolder.GetFiles("Journal.*.log")
                                                                  .Where(f => f.LastWriteTime.Date == DateTime.Now.Date) // .AddDays(-1)
                                                                  .OrderByDescending(f => f.LastWriteTime);

                Journal journal = new Journal(fileName);
                JournalEntryParser journalEntryParser = new JournalEntryParser();
                SquadronMissionSummarizer squadronMissionSummarizer = new SquadronMissionSummarizer();
                PilotState pilotState = new PilotState();
                SupportedFaction supportedFaction = new SupportedFaction(supportedFactionName, new string[0], new string[0]);

            foreach (SquadronSummaryEntry summaryEntry in journalFiles.Select(jf => jf.FullName)
                    .SelectMany(fileName => new Journal(fileName).Entries)
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
