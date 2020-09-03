using EDMissionSummary;
using EDMissionSummary.JournalEntryProcessors;
using EDMissionSummary.SummaryEntries;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummaryTest.JournalEntryProcessors
{
    public class TestDockedEntryProcessor
    {
        [Test]
        [TestCaseSource(nameof(ProcessSingleEntrySource))]
        public void ProcessSingleEntry(string journalEntry, string minorFaction, string expectedStationName, long expectedSystemAddress, string expectedSystemName, string expectedControllingMinorFaction)
        {
            DockedEntryProcessor dockedEventProcessor = new DockedEntryProcessor();
            PilotState pilotState = new PilotState();
            GalaxyState galaxyState = new GalaxyState();

            JObject entry = new JournalEntryParser().Parse(journalEntry);

            IEnumerable<SummaryEntry> entries = dockedEventProcessor.Process(pilotState, galaxyState, minorFaction, entry);
            Assert.That(entries, Is.Empty);
            Assert.That(pilotState.Missions, Is.Empty);
            Assert.That(pilotState.LastDockedStation, Is.Not.Null);
            Assert.That(pilotState.LastDockedStation.Name, Is.EqualTo(expectedStationName));
            Assert.That(pilotState.LastDockedStation.SystemAddress, Is.EqualTo(expectedSystemAddress));
            Assert.That(pilotState.LastDockedStation.ControllingMinorFaction, Is.EqualTo(expectedControllingMinorFaction));

            Assert.That(galaxyState.Systems, Has.Count.EqualTo(1));
            Assert.That(galaxyState.Systems[expectedSystemAddress], Is.EqualTo(expectedSystemName));

            Assert.That(galaxyState.Stations, Has.Count.EqualTo(1));
            Assert.That(galaxyState.Stations, Is.EquivalentTo(new[] { new Station(expectedStationName, expectedSystemAddress, expectedControllingMinorFaction) }));
        }

        public static IEnumerable ProcessSingleEntrySource()
        {
            yield return new TestCaseData(
                "{ 'timestamp':'2020-09-02T12:27:51Z', 'event':'Docked', 'StationName':'Hughes Enterprise', 'StationType':'Ocellus', 'StarSystem':'Kunti', 'SystemAddress':9468121064873, 'MarketID':3223869952, 'StationFaction':{ 'Name':'EDA Kunti League', 'FactionState':'Expansion' }, 'StationGovernment':'$government_PrisonColony;', 'StationGovernment_Localised':'Prison colony', 'StationServices':[ 'dock', 'autodock', 'commodities', 'contacts', 'exploration', 'missions', 'outfitting', 'crewlounge', 'rearm', 'refuel', 'repair', 'shipyard', 'tuning', 'engineer', 'missionsgenerated', 'facilitator', 'flightcontroller', 'stationoperations', 'powerplay', 'searchrescue', 'stationMenu', 'shop' ], 'StationEconomy':'$economy_Industrial;', 'StationEconomy_Localised':'Industrial', 'StationEconomies':[ { 'Name':'$economy_Industrial;', 'Name_Localised':'Industrial', 'Proportion':1.000000 } ], 'DistFromStarLS':580.732955 }"
                    .Replace("'", "\""),
                "",
                "Hughes Enterprise",
                9468121064873,
                "Kunti",
                "EDA Kunti League"
            );
            yield return new TestCaseData(
                "{ 'timestamp':'2020-09-02T12:47:00Z', 'event':'Docked', 'StationName':'Ansari Hangar', 'StationType':'Outpost', 'StarSystem':'9 G. Carinae', 'SystemAddress':560266725739, 'MarketID':3224745728, 'StationFaction':{ 'Name':'Purple Universal Network', 'FactionState':'War' }, 'StationGovernment':'$government_Corporate;', 'StationGovernment_Localised':'Corporate', 'StationServices':[ 'dock', 'autodock', 'commodities', 'contacts', 'exploration', 'missions', 'refuel', 'repair', 'tuning', 'engineer', 'missionsgenerated', 'flightcontroller', 'stationoperations', 'powerplay', 'searchrescue', 'stationMenu' ], 'StationEconomy':'$economy_Refinery;', 'StationEconomy_Localised':'Refinery', 'StationEconomies':[ { 'Name':'$economy_Refinery;', 'Name_Localised':'Refinery', 'Proportion':0.760000 }, { 'Name':'$economy_Extraction;', 'Name_Localised':'Extraction', 'Proportion':0.240000 } ], 'DistFromStarLS':2545.407850 }"
                    .Replace("'", "\""),
                "",
                "Ansari Hangar",
                560266725739,
                "9 G. Carinae",
                "Purple Universal Network"
            );
            
        }
    }
}
