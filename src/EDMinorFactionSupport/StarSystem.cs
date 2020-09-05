using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMinorFactionSupport
{
    public class StarSystem : IEquatable<StarSystem>
    {
        /// <summary>
        /// Create a new <see cref="StarSystem"/>.
        /// </summary>
        /// <param name="systemAdddress"></param>
        /// <param name="name"></param>
        /// <param name="controllingMinorFaction"></param>
        public StarSystem(long systemAdddress, string name, IEnumerable<string> minorFactions)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            SystemAdddress = systemAdddress;
            Name = name;
            MinorFactions = minorFactions;
        }


        /// <summary>
        /// The system address (unique, numerical ID).
        /// </summary>
        public long SystemAdddress { get; }

        /// <summary>
        /// The system name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The minor factions present at this station.
        /// </summary>
        public IEnumerable<string> MinorFactions { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as StarSystem);
        }

        public bool Equals(StarSystem other)
        {
            return other != null &&
                   SystemAdddress == other.SystemAdddress &&
                   Name == other.Name &&
                   MinorFactions.SequenceEqual(other.MinorFactions);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SystemAdddress, Name, MinorFactions);
        }
    }
}
