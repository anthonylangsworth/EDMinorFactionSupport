using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EDMissionSummary
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Station : IEquatable<Station>
    {
        /// <summary>
        /// Create a station.
        /// </summary>
        /// <param name="name">
        /// The station name. Should be unique within a system. Cannot be null or empty.
        /// </param>
        /// <param name="systemAddress">
        /// The system address, a unique identifier of the system.
        /// </param>
        /// <param name="controllingMinorFaction">
        /// The name of the controlling minor faction. Cannot be null or empty.
        /// </param>
        /// <exception cref="ArgumentException">
        /// No parameter can be empty or null.
        /// </exception>
        public Station(string name, long systemAddress, string controllingMinorFaction, IEnumerable<string> minorFactions)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }
            if (string.IsNullOrEmpty(controllingMinorFaction))
            {
                throw new ArgumentException($"'{nameof(controllingMinorFaction)}' cannot be null or empty", nameof(controllingMinorFaction));
            }

            Name = name;
            SystemAddress = systemAddress;
            ControllingMinorFaction = controllingMinorFaction;
            MinorFactions = minorFactions;
        }

        /// <summary>
        /// The station name. Should be unique within a system. Cannot be null or empty.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The name of the controlling minor faction. Cannot be null or empty.
        /// </summary>
        public string ControllingMinorFaction { get; }

        /// <summary>
        /// The minor factions present at this station.
        /// </summary>
        public IEnumerable<string> MinorFactions { get; }

        /// <summary>
        /// The system address, a unique identifier of the system.
        /// </summary>
        public long SystemAddress
        {
            get;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Station);
        }

        public bool Equals(Station other)
        {
            return other != null &&
                   Name == other.Name &&
                   ControllingMinorFaction == other.ControllingMinorFaction &&
                   SystemAddress == other.SystemAddress;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ControllingMinorFaction, SystemAddress);
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }

        public override string ToString()
        {
            return $"{ Name } ({ SystemAddress }) by { ControllingMinorFaction }";
        }
    }
}
