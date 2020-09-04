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
                1,
                "Name",
                "+"
            );
            Assert.That(mission.Id, Is.EqualTo(1));
            Assert.That(mission.Name, Is.EqualTo("Name"));
            Assert.That(mission.Influence, Is.EqualTo("+"));
        }
    }
}
