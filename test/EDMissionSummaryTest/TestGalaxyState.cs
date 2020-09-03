using EDMissionSummary;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMissionSummaryTest
{
    public class TestGalaxyState
    {
        [Test]
        public void Constructor()
        {
            GalaxyState galaxyState = new GalaxyState();
            Assert.That(galaxyState.Stations, Is.Empty);
            Assert.That(galaxyState.Systems, Is.Empty);
        }

        [Test]
        public void AddOrUpdateStation_Empty()
        {
            GalaxyState galaxyState = new GalaxyState();
            Station station = new Station("A", 1, "F1", new string[0]);
            galaxyState.AddOrUpdateStation(station);
            Assert.That(galaxyState.Stations, Is.EquivalentTo(new[] { station }));
            Assert.That(galaxyState.Systems, Is.Empty);
        }

        [Test]
        public void AddOrUpdateStation_Add()
        {
            GalaxyState galaxyState = new GalaxyState();
            Station station1 = new Station("A", 1, "F1", new string[0]);
            Station station2 = new Station("A", 2, "F2", new string[0]);
            galaxyState.AddOrUpdateStation(station1);
            galaxyState.AddOrUpdateStation(station2);
            Assert.That(galaxyState.Stations, Is.EquivalentTo(new[] { station1, station2 }));
            Assert.That(galaxyState.Systems, Is.Empty);
        }

        [Test]
        public void AddOrUpdateStation_Update()
        {
            GalaxyState galaxyState = new GalaxyState();
            Station station1 = new Station("A", 1, "F1", new string[0]);
            Station station2 = new Station("A", 1, "F2", new string[0]);
            galaxyState.AddOrUpdateStation(station1);
            galaxyState.AddOrUpdateStation(station2);
            Assert.That(galaxyState.Stations, Is.EquivalentTo(new[] { station2 }));
            Assert.That(galaxyState.Systems, Is.Empty);
        }

        [Test]
        public void GetStation_Empty()
        {
            GalaxyState galaxyState = new GalaxyState();
            Assert.That(galaxyState.GetStation("a", 1), Is.Null);
        }

        [Test]
        public void GetStation_Hit()
        {
            GalaxyState galaxyState = new GalaxyState();
            Station station1 = new Station("A", 1, "F1", new string[0]);
            Station station2 = new Station("B", 2, "F2", new string[0]);
            galaxyState.AddOrUpdateStation(station1);
            galaxyState.AddOrUpdateStation(station2);
            Assert.That(galaxyState.GetStation(station1.Name, station1.SystemAddress), Is.EqualTo(station1));
            Assert.That(galaxyState.GetStation(station2.Name, station2.SystemAddress), Is.EqualTo(station2));
        }

        [Test]
        public void GetStation_MissName()
        {
            GalaxyState galaxyState = new GalaxyState();
            Station station = new Station("A", 1, "F1", new string[0]);
            galaxyState.AddOrUpdateStation(station);
            Assert.That(galaxyState.GetStation(station.Name + "A", station.SystemAddress), Is.Null);
        }

        [Test]
        public void GetStation_MissSystemAddress()
        {
            GalaxyState galaxyState = new GalaxyState();
            Station station = new Station("A", 1, "F1", new string[0]);
            galaxyState.AddOrUpdateStation(station);
            Assert.That(galaxyState.GetStation(station.Name, station.SystemAddress + 1), Is.Null);
        }
    }
}
