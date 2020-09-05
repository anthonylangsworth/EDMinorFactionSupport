using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace EDMinorFactionSupport
{
    /// <summary>
    /// The state of the galaxy as discoverd by journal entries.
    /// </summary>
    public class GalaxyState
    {
        private HashSet<Station> _stations;

        /// <summary>
        /// Create a new <see cref="GalaxyState"/>.
        /// </summary>
        public GalaxyState()
        {
            _stations = new HashSet<Station>();
            Systems = new Dictionary<long, StarSystem>();
        }

        /// <summary>
        /// Either add a new station, if it is not known, or update the existing station, otherwise.
        /// A station is idenitfied by its name and system address.
        /// </summary>
        /// <param name="station">
        /// The station to add. This cannot be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="station"/> cannot be null.
        /// </exception>
        public void AddOrUpdateStation(Station station)
        {
            if (station is null)
            {
                throw new ArgumentNullException(nameof(station));
            }

            _stations.RemoveWhere(
                s => s.Name == station.Name && s.SystemAddress == station.SystemAddress);
            _stations.Add(station);
        }

        /// <summary>
        /// Find a station with the given name and system address.
        /// </summary>
        /// <param name="stationName">
        /// The station name. This cannot be null.
        /// </param>
        /// <param name="systemAddress">
        /// The system address.
        /// </param>
        /// <returns>
        /// The matching <see cref="Station"/> or null, if none match.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stationName"/> cannot be null.
        /// </exception>
        public Station GetStation(string stationName, long systemAddress)
        {
            if (stationName is null)
            {
                throw new ArgumentNullException(nameof(stationName));
            }

            return _stations.FirstOrDefault(
                 s => s.Name == stationName && s.SystemAddress == systemAddress);
        }

        /// <summary>
        /// Lookup the system name.
        /// </summary>
        /// <param name="systemAddress">
        /// The system address.
        /// </param>
        /// <returns></returns>
        public string GetSystemName(long systemAddress)
        {
            string result;

            if(Systems.TryGetValue(systemAddress, out StarSystem system))
            {
                result = system.Name;
            }
            else
            {
                result = systemAddress.ToString();
            }

            return result;
        }


        public IEnumerable<Station> Stations => _stations;

        public IDictionary<long, StarSystem> Systems
        {
            get;
        }
    }
}
