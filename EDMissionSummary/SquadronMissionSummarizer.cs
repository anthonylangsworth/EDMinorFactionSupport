using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDMissionSummary
{
    public class SquadronMissionSummarizer
    {
        public readonly string MissionCompletedEvent = "MissionCompleted";
        public readonly string FactionEffectsSectionName = "FactionEffects";

        public SquadronMissionSummarizer(string supportedFaction)
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

        public SquadronSummaryEntry Convert(JObject entry)
        {
            if(entry == null)
            {
                throw new NullReferenceException(nameof(entry));
            }

            if (entry.Value<string>("event") == MissionCompletedEvent)
            {
                // entry.Property(FactionEffectsSectionName);

                return new SquadronSummaryMissionEntry(entry.Value<string>("DestinationSystem"), entry.Value<string>("Faction"), 0);
            }
            else
            {
                return null;
            }
        }
    }
}
