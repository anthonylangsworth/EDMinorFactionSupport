using NUnit.Framework;
using EDMissionSummary.SummaryEntries;
using System.Text;
using EDMissionSummary.JournalSources;
using EDMissionSummary;
using System.Linq;
using EDMissionSummary.JournalEntryProcessors;
using System.Collections;
using System.Resources;
using System.Reflection;
using System.IO;

namespace EDMissionSummaryTest
{
    public class JournalTests
    {
        [Test]
        [TestCaseSource(nameof(Source))]
        public void Test(string journal, SummaryEntry[] expectedSummaryEntries)
        {
            PilotState pilotState = new PilotState();
            GalaxyState galaxyState = new GalaxyState();
            string supportedFaction = "EDA Kunti League";

            Assert.That(
                new StringJournalSource(journal).Entries
                                                .Select(new JournalEntryParser().Parse)
                                                .SelectMany(entry => new MissionSummarizer().Convert(pilotState, galaxyState, supportedFaction, entry)), 
                Is.EquivalentTo(expectedSummaryEntries));
        }

        public static IEnumerable Source()
        {
            yield return new TestCaseData(LoadTestJournal("Empty.log"), new SummaryEntry[0]);
        }

        public static string LoadTestJournal(string name)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("EDMissionSummaryTest.TestJournals." + name))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
