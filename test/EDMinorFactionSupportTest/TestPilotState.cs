using EDMinorFactionSupport;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDMinorFactionSupportTest
{
    public class TestPilotState
    {
        [Test]
        public void Constructor()
        {
            PilotState pilotState = new PilotState();
            Assert.That(pilotState.Missions, Is.Empty, "Missions is not empty");
            Assert.That(pilotState.LastDockedStation, Is.Null);
        }
    }
}
