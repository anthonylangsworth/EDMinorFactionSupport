using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EDMinorFactionSupport;
using EDMinorFactionSupport.JournalEntryProcessors;
using EDMinorFactionSupport.SummaryEntries;
using EDMinorFactionSupport.JournalSources;
using System.Text;
using System.Text.RegularExpressions;

namespace edmfs
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
            JournalSource journal = new EdFileJournalSource(DateTime.Now.AddDays(-3)); 
            // JournalSource journal = new EdFileJournalSource(DateTime.MinValue); // Does all files
            // JournalSource journal = new FileJournalSource(fileName);
            JournalEntryParser journalEntryParser = new JournalEntryParser();
            Summarizer missionSummarizer = new Summarizer();
            PilotState pilotState = new PilotState();
            GalaxyState galaxyState = new GalaxyState();
            string supportedMinorFaction = "EDA Kunti League";

            IEnumerable<SummaryEntry> summary = journal.Entries
                                                       .Select(journalEntryParser.Parse)
                                                       .SelectMany(entry => missionSummarizer.Convert(pilotState, galaxyState, supportedMinorFaction, entry));
            // Verbose output
            // Console.Out.WriteLine(summary.Aggregate(new StringBuilder(), (sb, se) => sb.AppendLine(se.ToString())));

            // Summarized output
            Console.Out.WriteLine(summary.GroupBy(se => se.SystemName)
                                         .Select(grp => string.Format(
                                            "{0}\n{1}", 
                                            grp.Key, 
                                            DisplayByFactionSupport(grp)))
                                         .Aggregate(new StringBuilder(), (sb, line) => sb.Append(line))
                                         .ToString());

            //}
            //catch(Exception ex)
            //{
            //    Console.Error.Write(ex.ToString());
            //}
        }

        static string DisplayByFactionSupport(IEnumerable<SummaryEntry> summaryEntries)
        {
            return summaryEntries.GroupBy(se => se.IncreasesInfluence)
                                 .Select(grp => string.Format(
                                     "{0}\n{1}{2}", 
                                     grp.Key ? "PRO" : "CON", 
                                     DisplayMissions(grp.Where(se => se is MissionSummaryEntry).Cast<MissionSummaryEntry>()),
                                     DisplayVouchers(grp.Where(se => se is RedeemVoucherSummaryEntry).Cast<RedeemVoucherSummaryEntry>())))
                                 .Aggregate(new StringBuilder(), (sb, line) => sb.AppendLine(line))
                                 .ToString();
        }

        static string DisplayMissions(IEnumerable<MissionSummaryEntry> missionSummaryEntries)
        {
            return missionSummaryEntries.GroupBy(mse => mse.Influence)
                                        .Select(grp => string.Format("{0} Inf{1}", grp.Count(), grp.Key))
                                        .Aggregate(new StringBuilder(), (sb, line) => sb.AppendLine(line))
                                        .ToString();
        }

        static string DisplayVouchers(IEnumerable<RedeemVoucherSummaryEntry> redeemVoucherSummaryEntries)
        {
            return redeemVoucherSummaryEntries.GroupBy(mse => mse.VoucherType)
                                              .Select(grp => string.Format("{0} {1:n0} CR", SplitOnUpper(grp.Key), grp.Sum(rvse => rvse.Amount)))
                                              .Aggregate(new StringBuilder(), (sb, line) => sb.AppendLine(line))
                                              .ToString();
        }

        static string SplitOnUpper(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException($"'{nameof(s)}' cannot be null or empty", nameof(s));
            }

            StringBuilder stringBuilder = new StringBuilder();
            s.Take(1).Aggregate(stringBuilder, (sb, c) => sb.Append(char.ToUpper(c)));
            s.Skip(1).Aggregate(stringBuilder, (sb, c) => sb.Append(char.IsUpper(c) ? $"{ c } " : $"{ c }"));
            return stringBuilder.ToString();
        }
    }
}
