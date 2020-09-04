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
            Missions = new Dictionary<long, Mission>();
        }

        public Station LastDockedStation 
        {
            get;
            set;
        }

        public IDictionary<long, Mission> Missions
        {
            get;
        }
    }
}
