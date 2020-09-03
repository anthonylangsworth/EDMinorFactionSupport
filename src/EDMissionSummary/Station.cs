using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary
{
    public class Station
    {
        public Station(string name, ulong systemAddress, string controllingMinorFaction)
        {

        }

        public string Name
        {
            get; 
            set;
        }

        public string ControllingMinorFaction
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }
    }
}
