using EDMinorFactionSupport;
using EDMinorFactionSupport.JournalEntryProcessors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EDMinorFactionSupportTest
{
    public class TestMissionSummarizer
    {
        [Test]
        public void EnsureAllJournEntryProcessorsReferenced()
        {
            IReadOnlyDictionary<string, JournalEntryProcessor> journalEntryProcessors = new Summarizer().JournalEntryProcessors;
            Assert.That(
                Assembly.GetAssembly(typeof(JournalEntryProcessor))
                        .GetTypes()
                        .Where(t => t != typeof(JournalEntryProcessor) && t.IsAssignableFrom(typeof(JournalEntryProcessor)) && !journalEntryProcessors.Values.Any(jep => jep.GetType() == t))
                        .Select(t => t.FullName),
                Is.Empty, 
                "Have you forgotten to include a JournalEntryProcessor?");
        }
    }
}
