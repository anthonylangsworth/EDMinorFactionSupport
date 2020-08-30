using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary
{
    public class SquadronMissionSummary
    {
        public readonly string MissionCompletedEvent = "MissionCompleted";

        public SquadronMissionSummary(string supportedFaction)
        {
            if(string.IsNullOrWhiteSpace(supportedFaction))
            {
                throw new ArgumentException("Cannot be null or whitespace", nameof(supportedFaction));
            }

            SupportedFaction = supportedFaction;
        }

        public string SupportedFaction
        {
            get;
        }

        public Dictionary<string, string> Convert(JObject entry)
        {
            if(entry == null)
            {
                throw new NullReferenceException(nameof(entry));
            }

            if (entry.Value<string>("event") == MissionCompletedEvent
                && entry.Value<string>("faction") == SupportedFaction)
            { 

            }
        }
    }
}
