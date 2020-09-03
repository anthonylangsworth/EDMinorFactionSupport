using EDMissionSummary;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummaryTest
{
    public class TestMission
    {
        [Test]
        public void Constructor()
        {
            Mission mission = new Mission(
                "MissionID",
                "Name",
                "SourceFactionName",
                "TargetFactionName",
                "DestinationSystem",
                "DestinationStation");
            Assert.That(mission.DestinationStation, Is.EqualTo("DestinationStation"));
            Assert.That(mission.DestinationSystem, Is.EqualTo("DestinationSystem"));
            Assert.That(mission.MissonId, Is.EqualTo("MissionID"));
            Assert.That(mission.Name, Is.EqualTo("Name"));
            Assert.That(mission.SourceFactionName, Is.EqualTo("SourceFactionName"));
            Assert.That(mission.TargetFactionName, Is.EqualTo("TargetFactionName"));
        }
    }
}
