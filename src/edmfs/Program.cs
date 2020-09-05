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
using System.CodeDom.Compiler;
using EDMinorFactionSupport.OutputFormatters;

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
                .AddTransient(typeof(JournalSource), sp => new EdFileJournalSource(DateTime.Now.AddDays(-6)))
                //.AddTransient<JournalSource, EdFileJournalSource>()
                //.AddTransient(typeof(JournalSource), sp => new FileJournalSource(fileName))
                // .AddTransient<OutputFormatter, VerboseOutputFormatter>()
                .AddTransient<OutputFormatter, StandardOutputFormatter>()
                .AddTransient<Pipeline, Pipeline>()
                .BuildServiceProvider();

            string supportedMinorFaction = "EDA Kunti League";
            Pipeline pipeline = serviceProvider.GetService<Pipeline>();
            pipeline.Run(supportedMinorFaction, Console.Out);

            //}
            //catch(Exception ex)
            //{
            //    Console.Error.Write(ex.ToString());
            //}
        }
    }
}
