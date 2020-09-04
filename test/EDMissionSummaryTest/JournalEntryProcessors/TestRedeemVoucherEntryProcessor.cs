using EDMissionSummary;
using EDMissionSummary.JournalEntryProcessors;
using EDMissionSummary.SummaryEntries;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EDMissionSummaryTest.JournalEntryProcessors
{
    public class TestRedeemVoucherEntryProcessor
    {
        [Test]
        [TestCaseSource(nameof(ProcessSingleEntrySource))]
        public void ProcessSingleEntry(string journalEntry, string supportedMinorFaction, IEnumerable<RedeemVoucherSummaryEntry> expectedSummaryEntries)
        {
            RedeemVoucherEntryProcessor dockedEventProcessor = new RedeemVoucherEntryProcessor();
            PilotState pilotState = new PilotState();
            pilotState.LastDockedStation = new Station("Quetelet Dock", 1, "The Sovereign Justice Collective"); 
            GalaxyState galaxyState = new GalaxyState();
            galaxyState.Systems[1] = new StarSystem(1, "Afli", new string[] { "The Sovereign Justice Collective", "Afli Silver Universal Exchange" });

            JObject entry = new JournalEntryParser().Parse(journalEntry);
                
            Assert.That(
                dockedEventProcessor.Process(pilotState, galaxyState, supportedMinorFaction, entry).Cast<RedeemVoucherSummaryEntry>(),
                Is.EquivalentTo(expectedSummaryEntries));
        }

        public static IEnumerable ProcessSingleEntrySource()
        {
            yield return new TestCaseData(
                "{ 'timestamp':'2020-07-17T08:33:09Z', 'event':'RedeemVoucher', 'Type':'bounty', 'Amount':107549, 'Factions':[ { 'Faction':'The Sovereign Justice Collective', 'Amount':107549 } ] }"
                    .Replace("'", "\""),
                "The Sovereign Justice Collective",
                new RedeemVoucherSummaryEntry[] 
                { 
                    new RedeemVoucherSummaryEntry(JournalEntryProcessor.ParseTimeStamp("2020-07-17T08:33:09Z"), "Afli", true, "bounty", 107549) 
                });
            yield return new TestCaseData(
                "{ 'timestamp':'2020-07-17T08:33:09Z', 'event':'RedeemVoucher', 'Type':'bounty', 'Amount':107549, 'Factions':[ { 'Faction':'The Sovereign Justice Collective', 'Amount':107549 } ] }"
                    .Replace("'", "\""),
                "Afli Silver Universal Exchange",
                new RedeemVoucherSummaryEntry[] 
                {  
                    new RedeemVoucherSummaryEntry(JournalEntryProcessor.ParseTimeStamp("2020-07-17T08:33:09Z"), "Afli", false, "bounty", 107549)
                });
            yield return new TestCaseData(
                "{ 'timestamp':'2020-07-17T08:33:09Z', 'event':'RedeemVoucher', 'Type':'bounty', 'Amount':128024, 'Factions':[ { 'Faction':'The Sovereign Justice Collective', 'Amount':107549 }, { 'Faction':'Afli Silver Universal Exchange', 'Amount':20475 } ] }"
                    .Replace("'", "\""),
                "The Sovereign Justice Collective",
                new RedeemVoucherSummaryEntry[] 
                { 
                    new RedeemVoucherSummaryEntry(JournalEntryProcessor.ParseTimeStamp("2020-07-17T08:33:09Z"), "Afli", true, "bounty", 107549), 
                    new RedeemVoucherSummaryEntry(JournalEntryProcessor.ParseTimeStamp("2020-07-17T08:33:09Z"), "Afli", false, "bounty", 20475)
                });
            yield return new TestCaseData(
                "{ 'timestamp':'2020-07-17T08:33:09Z', 'event':'RedeemVoucher', 'Type':'bounty', 'Amount':128024, 'Factions':[ { 'Faction':'The Sovereign Justice Collective', 'Amount':107549 }, { 'Faction':'Afli Silver Universal Exchange', 'Amount':20475 } ] }"
                    .Replace("'", "\""),
                "Afli Silver Universal Exchange",
                new RedeemVoucherSummaryEntry[] 
                {
                    new RedeemVoucherSummaryEntry(JournalEntryProcessor.ParseTimeStamp("2020-07-17T08:33:09Z"), "Afli", false, "bounty", 107549),
                    new RedeemVoucherSummaryEntry(JournalEntryProcessor.ParseTimeStamp("2020-07-17T08:33:09Z"), "Afli", true, "bounty", 20475)
                });
            yield return new TestCaseData(
                "{ 'timestamp':'2020-07-17T12:18:21Z', 'event':'RedeemVoucher', 'Type':'CombatBond', 'Amount':598144, 'Faction':'The Sovereign Justice Collective' }"
                    .Replace("'", "\""),
                "The Sovereign Justice Collective",
                new RedeemVoucherSummaryEntry[] 
                { 
                    new RedeemVoucherSummaryEntry(JournalEntryProcessor.ParseTimeStamp("2020-07-17T12:18:21Z"), "Afli", true, "CombatBond", 598144) 
                });
            yield return new TestCaseData(
                "{ 'timestamp':'2020-07-17T12:18:21Z', 'event':'RedeemVoucher', 'Type':'CombatBond', 'Amount':598144, 'Faction':'The Sovereign Justice Collective' }"
                    .Replace("'", "\""),
                "Afli Silver Universal Exchange",
                new RedeemVoucherSummaryEntry[]
                {
                    new RedeemVoucherSummaryEntry(JournalEntryProcessor.ParseTimeStamp("2020-07-17T12:18:21Z"), "Afli", false, "CombatBond", 598144)
                });
        }

        [Test]
        [TestCaseSource(nameof(GetFactionInfluenceSource))]
        public FactionInfluence GetFactionInfluence(string supportedMinorFaction, string entryFaction, string stationControllingFaction, IEnumerable<string> stationMinorFactions)
        {
            return RedeemVoucherEntryProcessor.GetFactionInfluence(supportedMinorFaction, entryFaction, stationControllingFaction, stationMinorFactions);
        }

        public static IEnumerable GetFactionInfluenceSource()
        {
            return new TestCaseData[]
            {
                new TestCaseData("A", "A", "C", new string[0]).Returns(FactionInfluence.Increase),
                new TestCaseData("A", "B", "A", new string[0]).Returns(FactionInfluence.None),
                new TestCaseData("A", "B", "A", new string[] { "B" }).Returns(FactionInfluence.Decrease),
                new TestCaseData("A", "B", "A", new string[] { "B", "C" }).Returns(FactionInfluence.Decrease),
                new TestCaseData("A", "B", "B", new string[] { "A" }).Returns(FactionInfluence.Decrease)
            };
        }
    }
}
