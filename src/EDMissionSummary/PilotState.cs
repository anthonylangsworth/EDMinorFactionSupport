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
            LastDockedStation = null;
            Missions = new HashSet<Mission>();
        }

        public Station LastDockedStation 
        {
            get;
            set;
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
