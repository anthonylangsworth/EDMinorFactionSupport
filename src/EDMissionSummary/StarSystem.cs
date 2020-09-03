using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary
{
    public class StarSystem : IEquatable<StarSystem>
    {
        /// <summary>
        /// Create a new <see cref="StarSystem"/>.
        /// </summary>
        /// <param name="systemAdddress"></param>
        /// <param name="name"></param>
        /// <param name="controllingMinorFaction"></param>
        public StarSystem(long systemAdddress, string name, string controllingMinorFaction)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            if (string.IsNullOrEmpty(controllingMinorFaction))
            {
                throw new ArgumentException($"'{nameof(controllingMinorFaction)}' cannot be null or empty", nameof(controllingMinorFaction));
            }

            SystemAdddress = systemAdddress;
            Name = name;
            ControllingMinorFaction = controllingMinorFaction;
        }

        public long SystemAdddress { get; }
        public string Name { get; }
        public string ControllingMinorFaction { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as StarSystem);
        }

        public bool Equals(StarSystem other)
        {
            return other != null &&
                   SystemAdddress == other.SystemAdddress &&
                   Name == other.Name &&
                   ControllingMinorFaction == other.ControllingMinorFaction;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SystemAdddress, Name, ControllingMinorFaction);
        }
    }
}
