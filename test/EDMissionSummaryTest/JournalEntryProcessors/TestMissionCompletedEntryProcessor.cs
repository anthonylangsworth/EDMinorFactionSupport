using EDMissionSummary;
using EDMissionSummary.JournalEntryProcessors;
using EDMissionSummary.SummaryEntries;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMissionSummaryTest.JournalEntryProcessors
{
    public class TestMissionCompletedEntryProcessor
    {
        [Test]
        [TestCaseSource(nameof(ProcessSingleEntrySource))]
        public void ProcessSingleEntry(string journalEntry, string minorFaction, Mission mission, StarSystem system, IEnumerable<MissionSummaryEntry> expectedSummaryEntries)
        {
            MissionCompletedEntryProcessor missionCompletedEventProcessor = new MissionCompletedEntryProcessor();
            PilotState pilotState = new PilotState();
            pilotState.Missions.Add(mission.Id, mission);
            GalaxyState galaxyState = new GalaxyState();
            galaxyState.Systems.Add(system.SystemAdddress, system);

            JObject entry = new JournalEntryParser().Parse(journalEntry);

            IEnumerable<SummaryEntry> entries = missionCompletedEventProcessor.Process(pilotState, galaxyState, minorFaction, entry);
            Assert.That(entries, Is.Empty);
        }

        public static IEnumerable ProcessSingleEntrySource()
        {
            yield return new TestCaseData(
                "{'timestamp': '2020-08-30T01:25:07Z', 'event': 'MissionAccepted', 'Faction': 'EDA Kunti League', 'Name': 'Mission_Delivery_Investment', 'LocalisedName': 'Improve our financial status by delivering 120 units of Food Cartridges', 'Commodity': '$FoodCartridges_Name;', 'Commodity_Localised': 'Food Cartridges', 'Count': 120, 'TargetFaction': 'LTT 2337 Empire Party', 'DestinationSystem': 'Kunti', 'DestinationStation': 'Hughes Enterprise', 'Expiry': '2020-08-31T01:23:51Z', 'Wing': false, 'Influence': '++', 'Reputation': '++', 'Reward': 1111394, 'MissionID': 624049090}"
                    .Replace("'", "\""),
                "",
                null,
                null,
                new MissionSummaryEntry[] { }
            );
        }
    }
}
