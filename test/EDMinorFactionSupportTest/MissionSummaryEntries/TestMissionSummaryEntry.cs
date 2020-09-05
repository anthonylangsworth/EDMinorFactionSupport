using EDMinorFactionSupport.SummaryEntries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDMinorFactionSupportTest.MissionSummaryEntries
{
    public class TestMissionSummaryEntry
    {
        public void Constructor()
        {
            MissionSummaryEntry missionSummaryEntry = new MissionSummaryEntry(DateTime.MinValue, "Mission Name", "System Name", true, "++");
            Assert.That(missionSummaryEntry.TimeStamp, Is.EqualTo(DateTime.MinValue));
            Assert.That(missionSummaryEntry.Influence, Is.EqualTo("++"));
            Assert.That(missionSummaryEntry.IncreasesInfluence, Is.EqualTo(true));
            Assert.That(missionSummaryEntry.SystemName, Is.EqualTo("System Name"));
            Assert.That(missionSummaryEntry.Name, Is.EqualTo("Mission Name"));
        }
    }
}
