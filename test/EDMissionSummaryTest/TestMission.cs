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
                "DestinationSystem");
            Assert.That(mission.DestinationSystem, Is.EqualTo("DestinationSystem"));
            Assert.That(mission.MissonId, Is.EqualTo("MissionID"));
            Assert.That(mission.Name, Is.EqualTo("Name"));
            Assert.That(mission.SourceMinorFactionName, Is.EqualTo("SourceFactionName"));
            Assert.That(mission.TargetMinorFactionName, Is.EqualTo("TargetFactionName"));
        }
    }
}
