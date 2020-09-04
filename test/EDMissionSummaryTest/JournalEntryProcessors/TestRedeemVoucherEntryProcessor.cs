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
        public void ProcessSingleEntry(string journalEntry, string minorFaction, IEnumerable<BountySummaryEntry> expectedSummaryEntries)
        {
            RedeemVoucherEntryProcessor dockedEventProcessor = new RedeemVoucherEntryProcessor();
            PilotState pilotState = new PilotState();
            GalaxyState galaxyState = new GalaxyState();

            JObject entry = new JournalEntryParser().Parse(journalEntry);

            Assert.That(
                dockedEventProcessor.Process(pilotState, galaxyState, minorFaction, entry).Cast<BountySummaryEntry>(),
                Is.EquivalentTo(expectedSummaryEntries));
        }

        public static IEnumerable ProcessSingleEntrySource()
        {
            yield return new TestCaseData(
                "{ 'timestamp':'2020-07-17T08:33:09Z', 'event':'RedeemVoucher', 'Type':'bounty', 'Amount':107549, 'Factions':[ { 'Faction':'The Sovereign Justice Collective', 'Amount':107549 } ] }"
                    .Replace("'", "\""),
                "The Sovereign Justice Collective",
                new BountySummaryEntry[] { new BountySummaryEntry(JournalEntryProcessor.ParseTimeStamp("2020-07-17T08:33:09Z"), 107549) });
            yield return new TestCaseData(
                "{ 'timestamp':'2020-07-17T08:33:09Z', 'event':'RedeemVoucher', 'Type':'bounty', 'Amount':107549, 'Factions':[ { 'Faction':'The Sovereign Justice Collective', 'Amount':107549 } ] }"
                    .Replace("'", "\""),
                "Afli Silver Universal Exchange",
                new BountySummaryEntry[] {  });
            yield return new TestCaseData(
                "{ 'timestamp':'2020-07-17T08:33:09Z', 'event':'RedeemVoucher', 'Type':'bounty', 'Amount':128024, 'Factions':[ { 'Faction':'The Sovereign Justice Collective', 'Amount':107549 }, { 'Faction':'Afli Silver Universal Exchange', 'Amount':20475 } ] }"
                    .Replace("'", "\""),
                "The Sovereign Justice Collective",
                new BountySummaryEntry[] { new BountySummaryEntry(JournalEntryProcessor.ParseTimeStamp("2020-07-17T08:33:09Z"), 107549) });
        }
    }
}
