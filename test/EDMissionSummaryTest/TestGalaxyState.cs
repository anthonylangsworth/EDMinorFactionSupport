using EDMissionSummary;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummaryTest
{
    public class TestGalaxyState
    {
        [Test]
        public void Constructor()
        {
            GalaxyState galaxyState = new GalaxyState();
            Assert.That(galaxyState.StationIdToName, Is.Empty);
        }
    }
}
