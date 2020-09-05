using EDMinorFactionSupport.JournalEntryProcessors;
using EDMinorFactionSupport.JournalSources;
using EDMinorFactionSupport.OutputFormatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace EDMinorFactionSupport
{
    public static class Extensions
    {
        public static IServiceCollection AddEDMinorFactionSupport(this IServiceCollection serviceCollection, DateTime? date, string supportedMinorFaction, bool verboseOutput)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }
            if (string.IsNullOrEmpty(supportedMinorFaction))
            {
                throw new ArgumentException($"'{nameof(supportedMinorFaction)}' cannot be null or empty", nameof(supportedMinorFaction));
            }

            // Debugging files
            // string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830134805.01.log";
            // string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830102216.01.log";
            // string fileName = @"C:\Users\antho\Saved Games\Frontier Developments\Elite Dangerous\Journal.200830212509.01.log";
            // string fileName = @"TestMissions.log";

            serviceCollection.TryAddEnumerable(
                Assembly.GetAssembly(typeof(JournalEntryProcessor))
                                          .GetTypes()
                                          .Where(t => !t.IsAbstract && typeof(JournalEntryProcessor).IsAssignableFrom(t))
                                          .Select(t => ServiceDescriptor.Scoped(typeof(JournalEntryProcessor), t)));
            serviceCollection
                .AddTransient<Summarizer, Summarizer>()
                .AddTransient<JournalEntryParser, JournalEntryParser>()
                .AddTransient(typeof(JournalSource), sp => new EdFileJournalSource(date.Value.Date))
                //.AddTransient<JournalSource, EdFileJournalSource>()
                //.AddTransient(typeof(JournalSource), sp => new FileJournalSource(fileName))
                .AddTransient<Pipeline, Pipeline>(
                    sp => new Pipeline(
                        sp.GetService<JournalSource>(), 
                        sp.GetService<JournalEntryParser>(), 
                        sp.GetService<Summarizer>(), 
                        sp.GetService<OutputFormatter>(), 
                        supportedMinorFaction));

            if (verboseOutput)
            {
                serviceCollection.AddTransient<OutputFormatter, VerboseOutputFormatter>();
            }
            else
            {
                serviceCollection.AddTransient<OutputFormatter, StandardOutputFormatter>();
            }

            return serviceCollection;
        }
    }
}
