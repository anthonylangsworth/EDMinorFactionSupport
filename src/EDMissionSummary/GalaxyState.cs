using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary
{
    public class GalaxyState
    {
        public GalaxyState()
        {
            StationIdToName = new Dictionary<string, string>();
        }

        public IDictionary<string, string> StationIdToName
        {
            get;
        }
    }
}
