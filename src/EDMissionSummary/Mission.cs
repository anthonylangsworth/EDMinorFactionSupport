﻿namespace EDMissionSummary
{
    public class Mission
    {
        public Mission(string missonId, string name, string sourceFactionName, string targetFactionName, string destinationSystem, string destinationStation, string influence)
        {
            TargetFactionName = targetFactionName;
            SourceFactionName = sourceFactionName;
            DestinationSystem = destinationSystem;
            DestinationStation = destinationStation;
            Name = name;
            Influence = influence;
            MissonId = missonId;
        }

        public string TargetFactionName
        {
            get;
        }

        public string SourceFactionName
        {
            get;
        }

        public string DestinationSystem
        {
            get;
        }

        public string DestinationStation
        {
            get;
        }

        public string Name
        {
            get;
        }

        public string Influence
        {
            get;
        }

        public string MissonId
        {
            get;
        }
    }
}