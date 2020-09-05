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
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.TryAddEnumerable(
                Assembly.GetAssembly(typeof(JournalEntryProcessor))
                                          .GetTypes()
                                          .Where(t => !t.IsAbstract && typeof(JournalEntryProcessor).IsAssignableFrom(t))
                                          .Select(t => ServiceDescriptor.Scoped(typeof(JournalEntryProcessor), t)));
            ServiceProvider serviceProvider = serviceCollection
                .AddTransient<Summarizer, Summarizer>()
                .AddTransient<JournalEntryParser, JournalEntryParser>()
                .AddTransient(typeof(JournalSource), sp => new EdFileJournalSource(DateTime.Now.AddDays(-3)))
                //.AddTransient<JournalSource, EdFileJournalSource>()
                //.AddTransient(typeof(JournalSource), sp => new FileJournalSource(fileName))
                .AddTransient<Pipeline, Pipeline>()
                .BuildServiceProvider();

            string supportedMinorFaction = "EDA Kunti League";
            Pipeline pipeline = serviceProvider.GetService<Pipeline>();
            IEnumerable<SummaryEntry> summary = pipeline.Run(supportedMinorFaction);

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
