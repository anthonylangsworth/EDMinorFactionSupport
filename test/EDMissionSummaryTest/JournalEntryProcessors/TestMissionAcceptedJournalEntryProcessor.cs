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
    public class TestMissionAcceptedJournalEntryProcessor
    {
        [Test]
        [TestCaseSource(nameof(ProcessSingleEntrySource))]
        public void ProcessSingleEntry(string journalEntry, string minorFaction, string expectedMissionID, string expectedName, string expectedSourceMinorFactionName, string expectedTargetMinorFactionName, string expectedDestinationSystem)
        {
            MissionAcceptedEntryProcessor missionAcceptedEventProcessor = new MissionAcceptedEntryProcessor();
            PilotState pilotState = new PilotState();
            GalaxyState galaxyState = new GalaxyState();

            JObject entry = new JournalEntryParser().Parse(journalEntry);

            IEnumerable<SummaryEntry> entries = missionAcceptedEventProcessor.Process(pilotState, galaxyState, minorFaction, entry);
            Assert.That(entries, Is.Empty);
            Assert.That(pilotState.LastDockedStation, Is.Null);
            Assert.That(pilotState.Missions, Has.Count.EqualTo(1));
            Assert.That(pilotState.Missions.First, Has.Property("MissonId").EqualTo(expectedMissionID));
            Assert.That(pilotState.Missions.First, Has.Property("Name").EqualTo(expectedName));
            Assert.That(pilotState.Missions.First, Has.Property("SourceMinorFactionName").EqualTo(expectedSourceMinorFactionName));
            Assert.That(pilotState.Missions.First, Has.Property("TargetMinorFactionName").EqualTo(expectedTargetMinorFactionName));
            Assert.That(pilotState.Missions.First, Has.Property("DestinationSystem").EqualTo(expectedDestinationSystem));
        }

        public static IEnumerable ProcessSingleEntrySource()
        {
            yield return new TestCaseData(
                "{'timestamp': '2020-08-30T01:25:07Z', 'event': 'MissionAccepted', 'Faction': 'EDA Kunti League', 'Name': 'Mission_Delivery_Investment', 'LocalisedName': 'Improve our financial status by delivering 120 units of Food Cartridges', 'Commodity': '$FoodCartridges_Name;', 'Commodity_Localised': 'Food Cartridges', 'Count': 120, 'TargetFaction': 'LTT 2337 Empire Party', 'DestinationSystem': 'Kunti', 'DestinationStation': 'Hughes Enterprise', 'Expiry': '2020-08-31T01:23:51Z', 'Wing': false, 'Influence': '++', 'Reputation': '++', 'Reward': 1111394, 'MissionID': 624049090}"
                    .Replace("'", "\""),
                "",
                "624049090",
                "Improve our financial status by delivering 120 units of Food Cartridges",
                "EDA Kunti League",
                "LTT 2337 Empire Party",
                "Kunti"
            );
            yield return new TestCaseData(
                "{ 'timestamp':'2020-09-02T10:57:31Z', 'event':'MissionAccepted', 'Faction':'LTT 2337 Empire Party', 'Name':'Mission_Assassinate_Legal_War', 'LocalisedName':'Assassinate Known Pirate: Blaise', 'TargetType':'$MissionUtil_FactionTag_PirateLord;', 'TargetType_Localised':'Known Pirate', 'TargetFaction':'LTT 2337 Jet Brothers', 'DestinationSystem':'LTT 2337', 'DestinationStation':'Weaver Laboratory', 'Target':'Blaise', 'Expiry':'2020-09-03T10:51:43Z', 'Wing':false, 'Influence':'++', 'Reputation':'++', 'Reward':2286683, 'MissionID': 624049090 }"
                    .Replace("'", "\""),
                "",
                "624049090",
                "Assassinate Known Pirate: Blaise",
                "LTT 2337 Empire Party",
                "LTT 2337 Jet Brothers",
                "LTT 2337"
            );
            yield return new TestCaseData(
                "{ 'timestamp':'2020-09-01T12:19:34Z', 'event':'MissionAccepted', 'Faction':'Kunti Central Limited', 'Name':'Mission_Delivery', 'LocalisedName':'Deliver 90 units of Survival Equipment', 'Commodity':'$SurvivalEquipment_Name;', 'Commodity_Localised':'Survival Equipment', 'Count':90, 'TargetFaction':'EDA Kunti League', 'DestinationSystem':'LTT 2337', 'DestinationStation':'Hall Station', 'Expiry':'2020-09-02T12:16:35Z', 'Wing':false, 'Influence':'++', 'Reputation':'++', 'Reward':746104, 'MissionID':624892207 }"
                    .Replace("'", "\""),
                "",
                "624892207",
                "Deliver 90 units of Survival Equipment",
                "Kunti Central Limited",
                "EDA Kunti League",
                "LTT 2337"
            );
            

        }
    }
}
