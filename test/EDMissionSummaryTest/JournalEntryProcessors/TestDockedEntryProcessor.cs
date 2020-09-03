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
        public void ProcessSingleEntry(string journalEntry, string minorFaction, string expectedStatioName, long expectedSystemAddress, string expectedControllingMinorFaction)
        {
            DockedEntryProcessor dockedEventProcessor = new DockedEntryProcessor();
            PilotState pilotState = new PilotState();
            GalaxyState galaxyState = new GalaxyState();

            JObject entry = new JournalEntryParser().Parse(journalEntry);

            IEnumerable<SummaryEntry> entries = dockedEventProcessor.Process(pilotState, galaxyState, minorFaction, entry);
            Assert.That(entries, Is.Empty);
            Assert.That(pilotState.Missions, Is.Empty);
            Assert.That(pilotState.LastDockedStation, Is.Not.Null);
            Assert.That(pilotState.LastDockedStation.Name, Is.EqualTo(expectedStatioName));
            Assert.That(pilotState.LastDockedStation.SystemAddress, Is.EqualTo(expectedSystemAddress));
            Assert.That(pilotState.LastDockedStation.ControllingMinorFaction, Is.EqualTo(expectedControllingMinorFaction));
        }

        public static IEnumerable ProcessSingleEntrySource()
        {
            yield return new TestCaseData(
                "{ 'timestamp':'2020-09-02T12:27:51Z', 'event':'Docked', 'StationName':'Hughes Enterprise', 'StationType':'Ocellus', 'StarSystem':'Kunti', 'SystemAddress':9468121064873, 'MarketID':3223869952, 'StationFaction':{ 'Name':'EDA Kunti League', 'FactionState':'Expansion' }, 'StationGovernment':'$government_PrisonColony;', 'StationGovernment_Localised':'Prison colony', 'StationServices':[ 'dock', 'autodock', 'commodities', 'contacts', 'exploration', 'missions', 'outfitting', 'crewlounge', 'rearm', 'refuel', 'repair', 'shipyard', 'tuning', 'engineer', 'missionsgenerated', 'facilitator', 'flightcontroller', 'stationoperations', 'powerplay', 'searchrescue', 'stationMenu', 'shop' ], 'StationEconomy':'$economy_Industrial;', 'StationEconomy_Localised':'Industrial', 'StationEconomies':[ { 'Name':'$economy_Industrial;', 'Name_Localised':'Industrial', 'Proportion':1.000000 } ], 'DistFromStarLS':580.732955 }"
                    .Replace("'", "\""),
                "",
                "Hughes Enterprise",
                9468121064873,
                "EDA Kunti League"
            );
        }
    }
}
