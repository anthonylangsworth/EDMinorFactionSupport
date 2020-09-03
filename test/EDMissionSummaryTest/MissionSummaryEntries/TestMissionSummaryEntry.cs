using EDMissionSummary.SummaryEntries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummaryTest.MissionSummaryEntries
{
    public class TestMissionSummaryEntry
    {
        public void Constructor()
        {
            MissionSummaryEntry missionSummaryEntry = new MissionSummaryEntry("SystemName", true, "++");
            Assert.That(missionSummaryEntry.Influence, Is.EqualTo("++"));
            Assert.That(missionSummaryEntry.SupportsFaction, Is.EqualTo(true));
            Assert.That(missionSummaryEntry.SystemName, Is.EqualTo("SystemName"));
        }
    }
}
