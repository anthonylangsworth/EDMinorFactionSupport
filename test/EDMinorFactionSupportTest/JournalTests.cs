using NUnit.Framework;
using EDMinorFactionSupport.SummaryEntries;
using System.Text;
using EDMinorFactionSupport.JournalSources;
using EDMinorFactionSupport;
using System.Linq;
using EDMinorFactionSupport.JournalEntryProcessors;
using System.Collections;
using System.Resources;
using System.Reflection;
using System.IO;

namespace EDMinorFactionSupportTest
{
    public class JournalTests
    {
        [Test]
        [TestCaseSource(nameof(Source))]
        public void Test(string journal, SummaryEntry[] expectedSummaryEntries)
        {
            PilotState pilotState = new PilotState();
            GalaxyState galaxyState = new GalaxyState();
            string supportedMinorFaction = "EDA Kunti League";

            Assert.That(
                new StringJournalSource(journal).Entries
                                                .Select(new JournalEntryParser().Parse)
                                                .SelectMany(entry => new Summarizer().Convert(pilotState, galaxyState, supportedMinorFaction, entry)), 
                Is.EquivalentTo(expectedSummaryEntries));
        }

        public static IEnumerable Source()
        {
            yield return new TestCaseData(LoadTestJournal("Empty.log"), new SummaryEntry[0]);
        }

        public static string LoadTestJournal(string name)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("EDMinorFactionSupportTest.TestJournals." + name))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
