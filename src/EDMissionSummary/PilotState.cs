using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary
{
    /// <summary>
    /// Holds pilot state as journal entries are processed.
    /// </summary>
    public class PilotState
    {
        public PilotState()
        {
            Station = null;
            Missions = new HashSet<Mission>();
        }

        public Station Station
        {
            get;
        }

        public string SystemName
        {
            get;
        }

        public ISet<Mission> Missions
        {
            get;
        }

        // Ideas
        // * Current System
        // * ISet<Mission>
    }
}
