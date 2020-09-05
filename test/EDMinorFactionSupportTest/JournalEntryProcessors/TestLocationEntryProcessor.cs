using EDMinorFactionSupport;
using EDMinorFactionSupport.JournalEntryProcessors;
using EDMinorFactionSupport.SummaryEntries;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EDMinorFactionSupportTest.JournalEntryProcessors
{
    public class TestLocationEntryProcessor
    {
        [Test]
        [TestCaseSource(nameof(ProcessSingleEntrySource))]
        public void ProcessSingleEntry(string journalEntry, string minorFaction, string expectedStationName, long expectedSystemAddress, string expectedSystemName, string expectedStationControllingMinorFaction, string[] expectedMinorFactions, string expectedSystemMinorFaction)
        {
            LocationEntryProcessor locationEntryProcessor = new LocationEntryProcessor();
            PilotState pilotState = new PilotState();
            GalaxyState galaxyState = new GalaxyState();

            JObject entry = new JournalEntryParser().Parse(journalEntry);

            IEnumerable<SummaryEntry> entries = locationEntryProcessor.Process(pilotState, galaxyState, minorFaction, entry);
            Assert.That(entries, Is.Empty);
            Assert.That(pilotState.Missions, Is.Empty);
            Assert.That(pilotState.LastDockedStation, Is.Not.Null);
            Assert.That(pilotState.LastDockedStation.Name, Is.EqualTo(expectedStationName));
            Assert.That(pilotState.LastDockedStation.SystemAddress, Is.EqualTo(expectedSystemAddress));
            Assert.That(pilotState.LastDockedStation.ControllingMinorFaction, Is.EqualTo(expectedStationControllingMinorFaction));

            Assert.That(galaxyState.Systems, Has.Count.EqualTo(1));
            Assert.That(galaxyState.Systems[expectedSystemAddress], Is.EqualTo(new StarSystem(expectedSystemAddress, expectedSystemName, expectedMinorFactions)));

            Assert.That(galaxyState.Stations, Has.Count.EqualTo(1));
            Assert.That(galaxyState.Stations, Is.EquivalentTo(new[] { new Station(expectedStationName, expectedSystemAddress, expectedStationControllingMinorFaction) }));
        }

        public static IEnumerable ProcessSingleEntrySource()
        {
            yield return new TestCaseData(
                "{ 'timestamp':'2020 - 07 - 17T11: 34:25Z', 'event':'Location', 'Docked':true, 'StationName':'Pu City', 'StationType':'Coriolis', 'MarketID':3223894016, 'StationFaction':{ 'Name':'The Sovereign Justice Collective', 'FactionState':'Boom' }, 'StationGovernment':'$government_Dictatorship;', 'StationGovernment_Localised':'Dictatorship', 'StationServices':[ 'dock', 'autodock', 'commodities', 'contacts', 'exploration', 'missions', 'outfitting', 'crewlounge', 'rearm', 'refuel', 'repair', 'shipyard', 'tuning', 'engineer', 'missionsgenerated', 'flightcontroller', 'stationoperations', 'powerplay', 'searchrescue', 'stationMenu', 'shop', 'modulepacks' ], 'StationEconomy':'$economy_HighTech;', 'StationEconomy_Localised':'High Tech', 'StationEconomies':[ { 'Name':'$economy_HighTech;', 'Name_Localised':'High Tech', 'Proportion':0.890000 }, { 'Name':'$economy_Refinery;', 'Name_Localised':'Refinery', 'Proportion':0.110000 } ], 'StarSystem':'Afli', 'SystemAddress':3107576550106, 'StarPos':[35.31250,-78.03125,38.62500], 'SystemAllegiance':'Independent', 'SystemEconomy':'$economy_HighTech;', 'SystemEconomy_Localised':'High Tech', 'SystemSecondEconomy':'$economy_Refinery;', 'SystemSecondEconomy_Localised':'Refinery', 'SystemGovernment':'$government_Dictatorship;', 'SystemGovernment_Localised':'Dictatorship', 'SystemSecurity':'$SYSTEM_SECURITY_high;', 'SystemSecurity_Localised':'High Security', 'Population':90349309, 'Body':'Pu City', 'BodyID':50, 'BodyType':'Station', 'Factions':[ { 'Name':'HR 8829 Purple State Industries', 'FactionState':'War', 'Government':'Corporate', 'Influence':0.074223, 'Allegiance':'Empire', 'Happiness':'$Faction_HappinessBand2;', 'Happiness_Localised':'Happy', 'MyReputation':100.000000, 'ActiveStates':[ { 'State':'War' } ] }, { 'Name':'Afli Imperial Society', 'FactionState':'Boom', 'Government':'Patronage', 'Influence':0.134403, 'Allegiance':'Empire', 'Happiness':'$Faction_HappinessBand2;', 'Happiness_Localised':'Happy', 'MyReputation':100.000000, 'ActiveStates':[ { 'State':'Boom' } ] }, { 'Name':'Afli Power Co', 'FactionState':'None', 'Government':'Corporate', 'Influence':0.044132, 'Allegiance':'Independent', 'Happiness':'$Faction_HappinessBand2;', 'Happiness_Localised':'Happy', 'MyReputation':100.000000 }, { 'Name':'Afli Patrons Principles', 'FactionState':'War', 'Government':'Patronage', 'Influence':0.074223, 'Allegiance':'Empire', 'Happiness':'$Faction_HappinessBand2;', 'Happiness_Localised':'Happy', 'MyReputation':100.000000, 'ActiveStates':[ { 'State':'War' } ] }, { 'Name':'Afli Blue Partnership', 'FactionState':'None', 'Government':'Anarchy', 'Influence':0.031093, 'Allegiance':'Independent', 'Happiness':'$Faction_HappinessBand2;', 'Happiness_Localised':'Happy', 'MyReputation':13.124900 }, { 'Name':'The Sovereign Justice Collective', 'FactionState':'Boom', 'Government':'Dictatorship', 'Influence':0.589769, 'Allegiance':'Independent', 'Happiness':'$Faction_HappinessBand2;', 'Happiness_Localised':'Happy', 'MyReputation':100.000000, 'ActiveStates':[ { 'State':'Boom' } ] }, { 'Name':'Afli Silver Universal Exchange', 'FactionState':'None', 'Government':'Corporate', 'Influence':0.052156, 'Allegiance':'Independent', 'Happiness':'$Faction_HappinessBand2;', 'Happiness_Localised':'Happy', 'MyReputation':81.570503 } ], 'SystemFaction':{ 'Name':'The Sovereign Justice Collective', 'FactionState':'Boom' }, 'Conflicts':[ { 'WarType':'war', 'Status':'active', 'Faction1':{ 'Name':'HR 8829 Purple State Industries', 'Stake':'Cosmogonic Vision Relay', 'WonDays':0 }, 'Faction2':{ 'Name':'Afli Patrons Principles', 'Stake':'Weizsacker Installation', 'WonDays':0 } } ] }"
                    .Replace("'", "\""),
                "",
                "Pu City",
                3107576550106,
                "Afli",
                "The Sovereign Justice Collective",
                new [] { 
                    "HR 8829 Purple State Industries",
                    "Afli Imperial Society",
                    "Afli Power Co",
                    "Afli Patrons Principles",
                    "Afli Blue Partnership",
                    "The Sovereign Justice Collective",
                    "Afli Silver Universal Exchange" 
                },
                "The Sovereign Justice Collective"
            );
        }
    }
}
